using Csla;
using Csla.Data;
using System;
using System.Data;
using TapMango.Domain.Extensions;

namespace TapMango.Domain.BusinessObjects
{
    [Serializable]
    public class TapMangoPlantList : BusinessListBase<TapMangoPlantList, TapMangoPlant>
    {
        public TapMangoPlantList()
        {
        }

        [Serializable]
        internal sealed class Criteria : CriteriaBase<Criteria>
        {
            private int _id;
            private string _action;

            public Criteria() : base()
            {
            }

            public Criteria(int id)
            {
                this._id = id;
            }

            public int Id
            {
                get { return this._id; }
                set { this._id = value; }
            }

            public string Action
            {
                get { return this._action; }
                set { this._action = value; }
            }
        }

        private void DataPortal_Fetch(TapMangoPlant.Criteria criteria)
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    this.RaiseListChangedEvents = false;

                    cm.CommandText = "usp_TapMangoPlant";
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@action", criteria.Action);
                    cm.Parameters.AddWithValue("@Id", criteria.Id);

                    using (var dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        while (dr.Read())
                        {
                            this.Add(new TapMangoPlant(dr));
                        }
                    }

                    this.RaiseListChangedEvents = true;
                }
            });
        }

        private void DataPortal_Delete(Criteria criteria)
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_TapMangoPlant";
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@action", criteria.Action);
                    cm.Parameters.AddWithValue("@Id", criteria.Id);

                    cm.ExecuteNonQuery();
                }
            });
        }

        public static TapMangoPlantList GetAll()
        {
            var criteria = new TapMangoPlant.Criteria { Action = "select" };

            return DataPortal.Fetch<TapMangoPlantList>(criteria);
        }
    }
}