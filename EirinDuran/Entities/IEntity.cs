using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public interface IEntity<Model>
    {
        void UpdateWith(Model model);

        Model ToModel();

        string GetId();
    }
}
