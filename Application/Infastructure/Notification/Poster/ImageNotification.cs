using Core.Entities;
using Microsoft.Extensions.Configuration;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
               
                String dateAndTime = hpbStatistic.LastUpdate.ToString("dd.MM.yyyy") + " - " + hpbStatistic.LastUpdate.ToShortTimeString();
                
                image.Mutate(ctx => ctx.DrawText(dateAndTime, new Font(fontFamily, 92, FontStyle.Bold), Color.White, new PointF(230, 427)));

                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalTotalCases.ToString(), 
                                new Font(fontFamily, 130, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(125, 660)));
                image.Mutate(ctx => ctx.DrawText(hpbStatistic.LocalTotalNumberOfIndividualsInHospitals.ToString(), 
                                new Font(fontFamily, 130, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(100, 1110)));

                image.Mutate(ctx => ctx.DrawText("New Cases : "+ hpbStatistic.LocalNewCases.ToString(),
                                new Font(fontFamily, 40, FontStyle.Bold), Color.FromRgb(210, 9, 61), new PointF(80, 850)));
                String imageName = "status_update_" + hpbStatistic.Id + ".png";
                image.Save(imageName);
                return imageName;
            }            
            
        }
    }
}
