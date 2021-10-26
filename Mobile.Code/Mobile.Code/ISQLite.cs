using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Code
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
