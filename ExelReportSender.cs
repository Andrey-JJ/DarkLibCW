using DarkLibCW.Areas.Identity.Data;
using DarkLibCW.Models;
using DarkLibCW.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using Quartz;
using System.Net.Mail;
using System.Net;

namespace DarkLibCW
{
    public class ExelReportSender
    {
        string file_path_template;
        string file_path_report;
        private readonly AppDBContext _context;
        private readonly UserManager<DarkLibUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        int count = 0;

        public ExelReportSender(AppDBContext context, UserManager<DarkLibUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        async Task PrepareReport()
        {
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var issues = _context.Issues
                .Include(i => i.Book).ThenInclude(b => b.CatalogCard)
                .Include(i => i.Subscriber)
                .Include(i => i.Librarian)
                .ToList();

            count = issues.Count;

            string path = "/Reports/report_issues.xlsx";
            //Путь к файлу с результатом
            string result = $"/Reports/report_issuesAll.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);

            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Работник Р.Р.";
                excelPackage.Workbook.Properties.Title = "Список выдачи за 30 дней";
                excelPackage.Workbook.Properties.Subject = "Выдачи";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Issues"];
                int startLine = 3;

                var executedIssues = issues.Where(i => i.ReturnDate.AddDays(7) < DateTime.Now).ToList();


                foreach (var item in executedIssues)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = item.Id;
                    worksheet.Cells[startLine, 3].Value = item.Book.CatalogCard.Title;
                    worksheet.Cells[startLine, 4].Value = item.Subscriber.FullName;
                    worksheet.Cells[startLine, 5].Value = item.Librarian.FullName;
                    worksheet.Cells[startLine, 6].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 7].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 6].Value = item.IssueDate.Date;
                    worksheet.Cells[startLine, 7].Value = item.ReturnDate.Date;
                }

                await excelPackage.SaveAsAsync(fr);
                excelPackage.Dispose();
            }
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (File.Exists(file_path_report))
                    File.Delete(file_path_report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            await PrepareReport();

            if(count > 0)
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("an.orehow2000@yandex.ru", "Система автоматической отчетности");
                // кому отправляем
                MailAddress to = new MailAddress("andrey.orekhov.01@mail.ru");
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Отчет об всех";
                // текст письма
                m.Body = "<h2>Системой был сформирован и отправлен отчет об истекающих сроках действия ПО</h2>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 465);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("lukarus22@yandex.ru",
                "02012001DFd"); //eEaJVhzFh1K0jewPp66Q
                smtp.EnableSsl = true;
                // вкладываем файл в письмо
                m.Attachments.Add(new Attachment(_appEnvironment.WebRootPath + file_path_report));
                // отправляем асинхронно
                //smtp.SendCompleted += Test;
                await smtp.SendMailAsync(m);
                m.Dispose();

                count = 0;
            }
        }
    }
}
