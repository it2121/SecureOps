using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SecureOps.Models;

namespace SecureOps.Services
{
    public class PdfService
    {
        private readonly string _incidentImageRoot;

        public PdfService(IWebHostEnvironment env)
        {
            // e.g., wwwroot/IncidentImages
            _incidentImageRoot = Path.Combine(env.WebRootPath, "photos");
        }  // 🧾 Incident PDF generator
        public byte[] GenerateIncidentReport(Incident incident)
        {
            var folderPath = Path.Combine(_incidentImageRoot, incident.Id.ToString());
            var imageFiles = Directory.Exists(folderPath) ? Directory.GetFiles(folderPath) : Array.Empty<string>();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4); 

                    // Header
                    page.Header()
                        .Text("Incident Report")
                        .FontSize(22)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(12);

                        // Basic Info
                        // ---------------------- Incident Details Section ----------------------
                        col.Item().Column(detailsCol =>
                        {
                            detailsCol.Spacing(8);

                            // Section header with accent color
                            detailsCol.Item().Text("Incident Details")
                                .FontSize(16)
                                .Bold()
                                .FontColor(Colors.BlueGrey.Darken2)
                                .Underline();

                            // Optional dynamic text (e.g., confidentiality or company branding)
                            detailsCol.Item().Text($"Confidential - {incident.Company?.Name ?? "SecureOps"}")
                                .FontSize(10)
                                .Italic()
                                .FontColor(Colors.Grey.Darken1)
                                .AlignRight();

                            // Table-like layout using Rows
                            void AddField(string label, string value, string? color = null)
                            {
                                detailsCol.Item().Row(row =>
                                {
                                    row.RelativeItem(1)
                                       .Text(label)
                                       .Bold()
                                       .FontSize(12)
                                       .FontColor(Colors.BlueGrey.Darken1);

                                    row.RelativeItem(2)
                                       .Text(value ?? "N/A")
                                       .FontSize(12)
                                       .FontColor(color != null ? color : Colors.BlueGrey.Darken3);
                                });
                            }

                            // Core Fields
                            AddField("Title", incident.Title);
                            AddField("Description", incident.Description);
                            AddField("Status", incident.Status,
                                incident.Status switch
                                {
                                    "Reported" => Colors.Orange.Medium,
                                    "In Progress" => Colors.Amber.Darken1,
                                    "Verified" => Colors.Teal.Lighten1,
                                    "Resolved" => Colors.Green.Darken1,
                                    "Closed" => Colors.BlueGrey.Darken1,
                                    _ => Colors.BlueGrey.Darken3
                                });
                            AddField("Severity", incident.Severity,
                                incident.Severity switch
                                {
                                    "Low" => Colors.Green.Lighten2,
                                    "Medium" => Colors.Orange.Lighten2,
                                    "High" => Colors.Orange.Darken2,
                                    "Critical" => Colors.Red.Accent2,
                                    _ => Colors.BlueGrey.Darken3
                                });
                            AddField("Reported At", incident.ReportedAt?.ToString("dd/MM/yyyy hh:mm tt"));
                            AddField("Resolved At", incident.ResolvedAt?.ToString("dd/MM/yyyy hh:mm tt"));
                            AddField("Location", incident.Location);
                            AddField("Reported By", incident.Employee?.FullName);
                            AddField("Verified By", incident.VerifiedBy?.FullName);
                            AddField("Company", incident.Company?.Name);

                            // Line separator after main info
                            detailsCol.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            // Audit fields with lighter shading
                            detailsCol.Item().Row(row =>
                            {
                                row.RelativeItem(1)
                                   .Text("Created By / At")
                                   .Bold()
                                   .FontColor(Colors.BlueGrey.Darken1);

                                row.RelativeItem(2)
                                   .Text($"{incident.CreatedBy?.FullName ?? "N/A"} / {incident.CreatedAt?.ToString("dd/MM/yyyy hh:mm tt") ?? "N/A"}")
                                   .FontColor(Colors.BlueGrey.Darken2);
                            });

                            detailsCol.Item().Row(row =>
                            {
                                row.RelativeItem(1)
                                   .Text("Updated By / At")
                                   .Bold()
                                   .FontColor(Colors.BlueGrey.Darken1);

                                row.RelativeItem(2)
                                   .Text($"{incident.UpdatedBy?.FullName ?? "N/A"} / {incident.UpdatedAt?.ToString("dd/MM/yyyy hh:mm tt") ?? "N/A"}")
                                   .FontColor(Colors.BlueGrey.Darken2);
                            });

                            // Optional: Subtle bottom line for visual separation
                            detailsCol.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
                        });

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Images Section
                        col.Item().Text("Attached Photos").FontSize(14).Bold().Underline();
                        if (imageFiles.Length > 0)
                        {
                            for (int i = 0; i < imageFiles.Length; i += 2)
                            {
                                var rowImages = imageFiles.Skip(i).Take(2).ToList();

                                col.Item().Row(row =>
                                {
                                    foreach (var imagePath in rowImages)
                                    {
                                        try
                                        {
                                            var imageBytes = File.ReadAllBytes(imagePath);
                                            row.RelativeItem(1).Padding(5)
                                                .Border(1).BorderColor(Colors.Grey.Lighten2)
                                                .Height(200)
                                                .Image(imageBytes)
                                                .FitArea()
                                                .WithCompressionQuality(ImageCompressionQuality.High);
                                        }
                                        catch
                                        {
                                            row.RelativeItem(1)
                                                .Text($"Error loading {Path.GetFileName(imagePath)}")
                                                .FontColor(Colors.Red.Medium)
                                                .AlignCenter();
                                        }
                                    }

                                    if (rowImages.Count == 1)
                                        row.RelativeItem(1).Padding(5);
                                });
                            }
                        }
                        else
                        {
                            col.Item().Text("No photos available.")
                                .Italic()
                                .FontSize(11)
                                .FontColor(Colors.Grey.Medium);
                        }

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Follow-up Tasks
                        if (incident.FollowUpTasks != null && incident.FollowUpTasks.Any())
                        {
                            col.Item().Text("Follow-Up Tasks").FontSize(14).Bold().Underline();
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Task Title").SemiBold();
                                    header.Cell().Text("Assigned To").SemiBold();
                                    header.Cell().Text("Due Date").SemiBold();
                                });

                                //foreach (var task in incident.FollowUpTasks)
                                //{
                                //    table.Cell().Text(task.Title);
                                //    table.Cell().Text(task.Employee?.FullName);
                                //    table.Cell().Text(task.DueDate.ToString("dd/MM/yyyy"));
                                //}
                            });
                        }

                        //// Related Documents
                        //if (incident.RelatedDocuments != null && incident.RelatedDocuments.Any())
                        //{
                        //    col.Item().Text("Related Documents").FontSize(14).Bold().Underline();
                        //    foreach (var doc in incident.RelatedDocuments)
                        //    {
                        //        col.Item().Text($"• {doc.FileName} ({doc.FileType})");
                        //    }
                        //}

                        // Footer
                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        col.Item().Text("Generated by SecureOps").FontSize(10).Italic().AlignRight();
                    });
                });
            });

            return pdf.GeneratePdf();
        }
        public byte[] GenerateSafetyTaskReport(SafetyTask task)
        {
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);

                    page.Header()
                        .Text("Safety Task Report")
                        .FontSize(22)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Column(col =>
                        {
                            col.Spacing(10);

                            col.Item().Text($"Task Title: {task.Title}");
                            col.Item().Text($"Assigned To: {task.Employee?.FullName}");
                            col.Item().Text($"Due Date: {task.DueDate:d}");
                            col.Item().Text($"Description: {task.Description}");
                            col.Item().Text($"Status: {(task.Status.Equals("Done") ? "Done" : "Pending")}");

                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().Text("Generated by SecureOps").FontSize(10).Italic().AlignRight();
                        });
                });
            });

            return pdf.GeneratePdf();
        }
    }
}
