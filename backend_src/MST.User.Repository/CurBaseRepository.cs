using FreeSql;

namespace MST.User.Repository;

public class CurBaseRepository<TSource,TKey>:BaseRepository<TSource, TKey> where TSource : class
{
    public CurBaseRepository(IFreeSql fsql) : base(fsql, null, null) {}
}