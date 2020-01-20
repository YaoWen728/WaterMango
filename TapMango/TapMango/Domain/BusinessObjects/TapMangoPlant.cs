using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using TapMango.Domain.Extensions;

namespace TapMango.Domain.BusinessObjects
{
    [Serializable]
    public class TapMangoPlant : BusinessBase<TapMangoPlant>
    {
        public TapMangoPlant()
        {
            this.MarkAsChild();
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

        private void FillSelf(SafeDataReader reader)
        {
            this.Id = reader.GetInt32(IdProperty.FriendlyName);
            this.Name = reader.GetString(NameProperty.FriendlyName);
            this.Uri = reader.GetString(UriProperty.FriendlyName);
            this.CreatedOn = reader.GetDateTime(CreatedOnProperty.FriendlyName);
        }

        internal TapMangoPlant(SafeDataReader reader) : this()
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

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name, "Name");
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { LoadProperty(NameProperty, value); }
        }

        public static readonly PropertyInfo<string> UriProperty = RegisterProperty<string>(c => c.Uri, "Uri");
        public string Uri
        {
            get { return GetProperty(UriProperty); }
            set { LoadProperty(UriProperty, value); }
        }

        public static readonly PropertyInfo<DateTime> CreatedOnProperty = RegisterProperty<DateTime>(c => c.CreatedOn, "CreatedOn");
        public DateTime CreatedOn
        {
            get { return GetProperty(CreatedOnProperty); }
            set { LoadProperty(CreatedOnProperty, value); }
        }

        private WateringSessionList _sessions;
        public WateringSessionList Sessions
        {
            get
            {
                return _sessions ?? (_sessions = WateringSessionList.GetByTapMangoPlantId(Id));
            }
        }

        private void AddParametersTo(SqlCommand cm, string action)
        {
            cm.CommandType = CommandType.StoredProcedure;

            cm.Parameters.AddWithValue("@action", action);
            cm.Parameters.AddWithValue("@Id", this.Id);
            cm.Parameters.AddWithValue("@Name", this.Name);
            cm.Parameters.AddWithValue("@Uri", this.Uri);
            cm.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
        }

        private void DataPortal_Delete(TapMangoPlant.Criteria criteria)
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_TapMangoPlant";
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
                    cm.CommandText = "usp_TapMangoPlant";
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
                    cm.CommandText = "usp_TapMangoPlant";
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
            if (!this.IsDirty)
            {
                return;
            }

            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_TapMangoPlant";
                    this.AddParametersTo(cm, "update");

                    cm.Parameters["@Id"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();
                }
            });

            this.MarkOld();
        }

        private void DataPortal_Fetch(TapMangoPlant.Criteria criteria)
        {
            TransactionHandler.ProcessTransaction((tr) =>
            {
                using (var cm = tr.CreateCommand())
                {
                    cm.CommandText = "usp_TapMangoPlant";
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

        public static TapMangoPlant NewTapMangoPlant()
        {
            var tapMangoPlant = DataPortal.Create<TapMangoPlant>();

            return tapMangoPlant;
        }

        public static TapMangoPlant GetById(System.Int32 id)
        {
            var criteria = new Criteria { Id = id, Action = "getbyid" };

            return DataPortal.Fetch<TapMangoPlant>(criteria);
        }

        protected override object GetIdValue()
        {
            return this.Id;
        }
    }
}