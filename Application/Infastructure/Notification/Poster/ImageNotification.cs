using Core.Entities;
using ImageMagick;
using Microsoft.Extensions.Configuration;
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
            using MagickImage image = new MagickImage(Configuration["HBP:LatestUpdateImage"]);

            String dateAndTime = hpbStatistic.LastUpdate.ToString("dd.MM.yyyy") + " - " + hpbStatistic.LastUpdate.ToShortTimeString();
            _ = new Drawables()
            .FontPointSize(92.0)
            .Font("Arial", FontStyleType.Normal, FontWeight.Bold, FontStretch.ExtraExpanded)
            .StrokeColor(MagickColors.White)
            .FillColor(MagickColors.White)
            .TextAlignment(TextAlignment.Center)
            .Text(670, 500, dateAndTime)
            .Draw(image);

            MagickColor numbrColor = new MagickColor(210, 9, 61);
            _ = new Drawables()
            .FontPointSize(130.0)
            .Font("Arial", FontStyleType.Normal, FontWeight.Bold, FontStretch.ExtraExpanded)
            .StrokeColor(numbrColor)
            .FillColor(numbrColor)
            .TextAlignment(TextAlignment.Center)
            .Text(200, 770, hpbStatistic.LocalTotalCases.ToString())
            .Draw(image);

            _ = new Drawables()
            .FontPointSize(130.0)
            .Font("Arial", FontStyleType.Normal, FontWeight.Bold, FontStretch.ExtraExpanded)
            .StrokeColor(numbrColor)
            .FillColor(numbrColor)
            .TextAlignment(TextAlignment.Center)
            .Text(200, 1200, hpbStatistic.LocalTotalNumberOfIndividualsInHospitals.ToString())
            .Draw(image);

            String imageName = "status_update_" + hpbStatistic.Id+".png";
            image.Write(imageName);

            // lossless compression to reduce size
            FileInfo finalImage = new FileInfo(imageName);
            ImageOptimizer optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(finalImage);
            finalImage.Refresh();

            return imageName;
        }
    }
}
