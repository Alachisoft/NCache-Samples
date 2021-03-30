using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sample.CustomerService.Domain;

namespace Sample.CustomerService.Maps
{
    public class NCachePollingTableMap : ClassMapping<NCachePollingTable>
    {
        public NCachePollingTableMap()
        {
            Table("ncache_db_sync");
            Lazy(true);
            ComposedId(compId =>
            {
                compId.Property(x => x.cache_key, m => m.Column("cache_key"));
                compId.Property(x => x.cache_id, m => m.Column("cache_id"));
            });
            Property(x => x.modified, map =>
            {
                map.Column(map =>
                {
                    map.Name("modified");
                    map.Default((byte)0);
                });

            });
            Property(x => x.work_in_progress, map =>
            {
                map.Column(map =>
                {

                    map.Name("work_in_progress");
                    map.Default((byte)0);
                });
            });
        }
    }
}
