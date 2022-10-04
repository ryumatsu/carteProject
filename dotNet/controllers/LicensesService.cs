using Sabio.Data.Providers;
using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Licenses;
using Sabio.Models;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class LicensesService : ILicensesService
    {
        IDataProvider _data = null;// this is an instance of this data provider that is outside AddressService() in order to gain access to it
        public LicensesService(IDataProvider data) //cannot do all the work alone, need to pass data provider, type is dataProvider named data
        {
            _data = data;//assigned a private field (_data) to equal data, assigned a data pointer to dataProvider
        }

        public void DeleteById(int Id) 
        {
            string procname = "[dbo].[Licenses_Delete_ById]";
            _data.ExecuteNonQuery(procname, inputParamMapper: delegate (SqlParameterCollection col)
            { 
                col.AddWithValue("@Id", Id);
            },
           returnParameters: null);
        }

        public Organization GetByOrgId(int orgId) 
        {
            string procName = "[dbo].[Licenses_GetLicenses_ByOrg]";

            Organization license = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@OrgId", orgId);

            }, singleRecordMapper: delegate (IDataReader reader, short set) 

            {
                license = MapOrgLicenses(reader);
            }
            );
            return license;
        }

        public int Add(LicensesAddRequest model, int userId) 
        {
            int id = 0;

            string procName = "[dbo].[Licenses_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)

            { 
                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy",userId);


                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public Paged<Licenses> SelectByCreatedBy(int pageIndex, int pageSize, int createdById)
        {
            Paged<Licenses> pagedResult = null;

            List<Licenses> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Licenses_Select_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@CreatedBy", createdById);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Licenses license = MapLicenses(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (result == null)
                    {
                        result = new List<Licenses>();
                    }

                    result.Add(license);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<Licenses>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Licenses GetById(int id) 
        {
            string procName = "[dbo].[Licenses_Select_ById]";

            Licenses license = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, singleRecordMapper: delegate (IDataReader reader, short set) 
            {
                int startingIndex = 0;
                license = MapLicenses(reader, ref startingIndex);
            }
            );

            return license;
        }

        public Licenses GetByLicenseType(int LicenseTypeId) 
        {
            string procName = "[dbo].[Licenses_Select_ByLicenseTypeId]";

            Licenses license = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@LicenseTypeId", LicenseTypeId);

            }, singleRecordMapper: delegate (IDataReader reader, short set) 
            {
                int startingIndex = 0;
                license = MapLicenses(reader, ref startingIndex);
            }
            );
            return license;
        }

        public Paged<Licenses> GetAll(int pageIndex, int pageSize)
        {
            Paged<Licenses> pagedResult = null;

            List<Licenses> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Licenses_SelectAll]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
          {
              int startingIndex = 0;

              Licenses friend = MapLicenses(reader, ref startingIndex);
                    

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (result == null)
                    {
                        result = new List<Licenses>();
                    }
                    result.Add(friend);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<Licenses>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;

        }

        public Paged<Licenses> GetQuery(int pageIndex, int pageSize, string query)
        {
            Paged<Licenses> pagedResult = null;

            List<Licenses> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Licenses_Query]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Licenses friend = MapLicenses(reader, ref startingIndex);


                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (result == null)
                    {
                        result = new List<Licenses>();
                    }
                    result.Add(friend);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<Licenses>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;

        }

        public void Update(LicensesUpdateRequest model, int userId) 
        {
            string procName = "[dbo].[Licenses_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@CreatedBy", userId);
            },
            returnParameters: null);
        }

        private static Licenses MapLicenses(IDataReader reader, ref int startingIndex)
        {
            Licenses theLicense = new Licenses();
            theLicense.LicenseId = new LookUp();
            theLicense.LicenseStateId = new LookUp();
            theLicense.LicenseTypeId = new LookUp();
            theLicense.Employees = new Employees();
            theLicense.UserProfile = new UserProfileBase();

            theLicense.LicenseId.Id = reader.GetSafeInt32(startingIndex++);
            theLicense.LicenseStateId.Id = reader.GetSafeInt32(startingIndex++);
            theLicense.LicenseStateId.Name = reader.GetSafeString(startingIndex++);
            theLicense.LicenseTypeId.Id = reader.GetSafeInt32(startingIndex++);
            theLicense.LicenseTypeId.Name = reader.GetSafeString(startingIndex++);
            theLicense.LicenseNumber = reader.GetSafeString(startingIndex++);
            theLicense.DateExpires = reader.GetSafeDateTime(startingIndex++);
            theLicense.IsVerified = reader.GetSafeBool(startingIndex++);
            theLicense.CreatedBy = reader.GetSafeInt32(startingIndex++);
            theLicense.Email = reader.GetSafeString(startingIndex++);
            return theLicense;
        }

        private static Organization MapOrgLicenses(IDataReader reader)
        {
            Organization theLicense = new Organization();
            theLicense.License = new List<Licenses>();

            int startingIndex = 0;

            theLicense.Id = reader.GetSafeInt32(startingIndex++);
            theLicense.Name = reader.GetSafeString(startingIndex++);
            theLicense.License = reader.DeserializeObject<List<Licenses>>(startingIndex++);
            return theLicense;
        }

        private static void AddCommonParams(LicensesAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LicenseStateId", model.LicenseStateId);
            col.AddWithValue("@LicenseTypeId", model.LicenseType);
            col.AddWithValue("@LicenseNumber", model.LicenseNumber);
            col.AddWithValue("@DateExpires", model.DateExpires);
            col.AddWithValue("@IsVerified", model.IsVerified);
        }
    }
}
