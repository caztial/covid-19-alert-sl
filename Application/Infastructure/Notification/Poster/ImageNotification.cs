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

                image.Mutate(ctx => ctx.DrawText(dateAndTime, new Font(fontFamily, 60, FontStyle.Bold), Color.White, new PointF(50, 230)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalTotalCases.ToString(), 
                                new Font(fontFamily, 80, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(40, 350)));
                
                image.Mutate(ctx => ctx.DrawText("New Cases : "+ hpbStatistic.LocalNewCases.ToString(),
                                new Font(fontFamily, 25, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(30, 430)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalActiveCases.ToString(),
                                new Font(fontFamily, 80, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(40, 610)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalTotalNumberOfIndividualsInHospitals.ToString(),
                                new Font(fontFamily, 80, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(40, 850)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalRecoverd.ToString(),
                                new Font(fontFamily, 80, FontStyle.Bold), Color.ForestGreen, new PointF(40, 1110)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalDeaths.ToString(),
                                new Font(fontFamily, 90, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(80, 1355)));

                String imageName = "status_update_" + hpbStatistic.Id + ".jpg";

                image.Save(imageName);
                return imageName;
            }            
            
        }
    }
}
