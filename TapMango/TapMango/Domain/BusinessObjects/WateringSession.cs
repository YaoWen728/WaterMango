using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using TapMango.Domain.Extensions;

namespace TapMango.Domain.BusinessObjects
{
    [Serializable]
    public class WateringSession : BusinessBase<WateringSession>
    {
        public WateringSession()
        {
            this.MarkAsChild();
        }

        [Serializable]
        internal sealed class Criteria : CriteriaBase<Criteria>
        {
            private int _id;
            private int _tapMangoPlantId;
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

            public int TapMangoPlantId
            {
                get { return this._tapMangoPlantId; }
                set { this._tapMangoPlantId = value; }
            }

            public string Action
            {
                get { return this._action; }
                set { this._action = value; }
            }
        }

        private void FillSelf(SafeDataReader reader)
        {
            this.Id = reader.GetInt32(IdProperty.FriendlyName);
            this.TapMangoPlantId = reader.GetInt32(TapMangoPlantIdProperty.FriendlyName);
            this.StatusId = reader.GetInt32(StatusIdProperty.FriendlyName);
            this.StartTime = reader.GetDateTime(StartTimeProperty.FriendlyName);
            this.ExpectToCompleteOn = reader.GetDateTime(ExpectToCompleteOnProperty.FriendlyName);
            this.CreatedOn = reader.GetDateTime(CreatedOnProperty.FriendlyName);
        }

        internal WateringSession(SafeDataReader reader) : this()
        {
            FillSelf(reader);

            this.MarkOld();
        }

        public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(c => c.Id, "Id");
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<int> TapMangoPlantIdProperty = RegisterProperty<int>(c => c.TapMangoPlantId, "TapMangoPlantId");
        public int TapMangoPlantId
        {
            get { return GetProperty(TapMangoPlantIdProperty); }
            set { LoadProperty(TapMangoPlantIdProperty, value); }
        }

        public static readonly PropertyInfo<int> StatusIdProperty = RegisterProperty<int>(c => c.StatusId, "StatusId");
        public int StatusId
        {
            get { return GetProperty(StatusIdProperty); }
            set { LoadProperty(StatusIdProperty, value); }
        }

        public static readonly PropertyInfo<DateTime> StartTimeProperty = RegisterProperty<DateTime>(c => c.StartTime, "StartTime");
        public DateTime StartTime
        {
            get { return GetProperty(StartTimeProperty); }
            set { LoadProperty(StartTimeProperty, value); }
        }

        public static readonly PropertyInfo<DateTime> ExpectToCompleteOnProperty = RegisterProperty<DateTime>(c => c.ExpectToCompleteOn, "ExpectToCompleteOn");
        public DateTime ExpectToCompleteOn
        {
            get { return GetProperty(ExpectToCompleteOnProperty); }
            set { LoadProperty(ExpectToCompleteOnProperty, value); }
        }

        public static readonly PropertyInfo<DateTime> CreatedOnProperty = RegisterProperty<DateTime>(c => c.CreatedOn, "CreatedOn");
        public DateTime CreatedOn
        {
            get { return GetProperty(CreatedOnProperty); }
            set { LoadProperty(CreatedOnProperty, value); }
        }

        private void AddParametersTo(SqlCommand cm, string action)
        {
            cm.CommandType = CommandType.StoredProcedure;

            cm.Parameters.AddWithValue("@action", action);
            cm.Parameters.AddWithValue("@Id", this.Id);
            cm.Parameters.AddWithValue("@TapMangoPlantId", this.TapMangoPlantId);
            cm.Parameters.AddWithValue("@StatusId", this.StatusId);
            cm.Parameters.AddWithValue("@StartTime", this.StartTime);
            cm.Parameters.AddWithValue("@ExpectToCompleteOn", this.ExpectToCompleteOn);
            cm.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
        }

        private void DataPortal_Delete(WateringSession.Criteria criteria)
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_WateringSession";
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@action", "delete");
                    cm.Parameters.AddWithValue("@Id", criteria.Id);

                    cm.ExecuteNonQuery();

                    this.MarkNew();
                }
            });
        }

        public void DeleteSelf()
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_WateringSession";
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@action", "delete");
                    cm.Parameters.AddWithValue("@id", this.Id);

                    cm.ExecuteNonQuery();

                    this.MarkNew();
                }
            });
        }

        public void Insert()
        {
            if (!this.IsDirty)
            {
                return;
            }

            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_WateringSession";
                    this.AddParametersTo(cm, "insert");

                    cm.Parameters["@Id"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    this.Id = (int)cm.Parameters["@Id"].Value;
                }
            });

            this.MarkOld();
        }

        protected override void DataPortal_Insert()
        {
            Insert();
        }

        public void Update()
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_WateringSession";
                    this.AddParametersTo(cm, "update");

                    cm.ExecuteNonQuery();
                }
            });
        }

        private void DataPortal_Fetch(WateringSession.Criteria criteria)
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_WateringSession";
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@action", criteria.Action);
                    cm.Parameters.AddWithValue("@Id", criteria.Id);

                    using (var dr = new SafeDataReader(cm.ExecuteReader()))
                    {
                        if (dr.Read())
                        {
                            FillSelf(dr);
                        }
                    }
                }
            });
        }

        [RunLocal]
        protected override void DataPortal_Create()
        {
            this.MarkAsChild();
        }

        public static WateringSession NewSession()
        {
            var session = DataPortal.Create<WateringSession>();

            return session;
        }

        public static WateringSession GetById(System.Int32 id)
        {
            var criteria = new Criteria { Id = id, Action = "getbyid" };

            return DataPortal.Fetch<WateringSession>(criteria);
        }

        protected override object GetIdValue()
        {
            return this.Id;
        }
    }
}