using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Infastructure.Notification
{
    interface INotification
    {
        public void Publish(HpbStatistic hpbStatistic);
    }
}
