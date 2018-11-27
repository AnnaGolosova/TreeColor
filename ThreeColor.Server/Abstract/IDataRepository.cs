using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using ThreeColor.Data.Models;
using ThreeColor.Server.Helpers;

namespace ThreeColor.Server.Abstract
{
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataRepository
    {
        ReturnDataModel<List<Tests>> GetTests(bool includeDeleted);

        ReturnDataModel<Tests> GetTest(int testId);

        ReturnDataModel UpdateTest(Tests test);

        ReturnDataModel<List<Points>> GetPoints(int testId, bool includeDeleted);

        ReturnDataModel UpdatePoints(List<Points> points);

        ReturnDataModel AddResult(Results result);

        ReturnDataModel<int> AddTest(Tests result);

        ReturnDataModel AddPoints(List<Points> points);

        ReturnDataModel<List<Results>> GetResultsByTest(int testId);

        ReturnDataModel<List<Results>> GetLastResults();
    }
}