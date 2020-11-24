using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace System.Database
{
    public class EntityManagerFactory
    {
        public static IEntityManager CreateEntityManager()
        {
            return new EntityManager();
        }
    }
}
