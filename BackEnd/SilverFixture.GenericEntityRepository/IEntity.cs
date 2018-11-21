using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.GenericEntityRepository
{
    public interface IEntity<Model>
    {
        void UpdateWith(Model model);

        Model ToModel();
    }
}
