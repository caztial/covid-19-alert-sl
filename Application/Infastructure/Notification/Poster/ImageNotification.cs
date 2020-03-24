using Core.Entities;
using Microsoft.Extensions.Configuration;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;

namespace Application.Infastructure.Notification.Poster
{
    public class ImageNotification : INotification
    {
        private IConfiguration Configuration { get; set; }

        public ImageNotification(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void Publish(HpbStatistic hpbStatistic)
        {
            CreateStatusUpdatePoster(hpbStatistic);
        }

        public string CreateStatusUpdatePoster(HpbStatistic hpbStatistic)
        {
            using (Image image = Image.Load(Configuration["HBP:LatestUpdateImage"]))
            {
                FontCollection fonts = new FontCollection();
                FontFamily fontFamily = fonts.Install(Configuration["HBP:font"]);
               
                String dateAndTime = hpbStatistic.LastUpdate.ToString("dd.MM.yyyy") + " - " + hpbStatistic.LastUpdate.ToString("hh.mm tt");

                image.Mutate(ctx => ctx.DrawText(dateAndTime, new Font(fontFamily, 60, FontStyle.Bold), Color.White, new PointF(50, 275)));

                int totalActive = hpbStatistic.LocalTotalCases - hpbStatistic.LocalRecoverd;

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalTotalCases.ToString(), 
                                new Font(fontFamily, 80, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(40, 440)));
                
                image.Mutate(ctx => ctx.DrawText("New Cases : "+ hpbStatistic.LocalNewCases.ToString(),
                                new Font(fontFamily, 25, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(30, 600)));

                image.Mutate(ctx => ctx.DrawText(totalActive.ToString(),
                                new Font(fontFamily, 80, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(40, 720)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalTotalNumberOfIndividualsInHospitals.ToString(),
                                new Font(fontFamily, 80, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(40, 1025)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalRecoverd.ToString(),
                                new Font(fontFamily, 90, FontStyle.Bold), Color.ForestGreen, new PointF(70, 1300)));

                String imageName = "status_update_" + hpbStatistic.Id + ".jpg";

                image.Save(imageName);
                return imageName;
            }            
            
        }
    }
}
