using System;
using System.Collections.Generic;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor
{
    public static class GuidGenerator
    {
        private static readonly Dictionary<string, Guid> FilterItemGuids =
            new Dictionary<string, Guid>
            {
                {"803fbf56-600f-490f-8409-6413a891720d:school_type:Default:Total", new Guid("cb9b57e8-9965-4cb6-b61a-acc6d34b32be")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:Total:Total", new Guid("183f94c3-b5d7-4868-892d-c948e256744d")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:Gender:Male", new Guid("eb6013a7-6e69-4ab6-b51e-b7a3e68256ae")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:Gender:Female", new Guid("ab381336-5e81-4caa-9e59-6e9067e8fb04")},
                {"803fbf56-600f-490f-8409-6413a891720d:school_type:Default:State-funded primary", new Guid("d7e7e412-f462-444f-84ac-3454fa471cb8")},
                {"803fbf56-600f-490f-8409-6413a891720d:school_type:Default:State-funded secondary", new Guid("a9fe9fa6-e91f-460b-a0b1-66877b97c581")},
                {"803fbf56-600f-490f-8409-6413a891720d:school_type:Default:Special", new Guid("b3207d77-143b-43d5-8b48-32d29727e96f")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:FSM:FSM eligible", new Guid("278ce0cb-f1d2-4e8f-84d1-ad794352c925")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:FSM:FSM not eligible", new Guid("bc6e41d5-5078-4207-b0ae-69261ac192e0")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:FSM:FSM unclassified", new Guid("c649015c-8203-43aa-b424-dfb897766391")},
                {"803fbf56-600f-490f-8409-6413a891720d:characteristic:Ethnic group minor:Ethnicity Minor Irish", new Guid("5a18e770-73b9-4b4f-8c3c-b3130b810422")},
                
                {"568576e5-d386-450e-a8db-307b7061d0d8:school_type:Default:Total", new Guid("0b0daf53-3dd5-4b9b-913e-f4518f4afb96")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:school_type:DefaultState-funded primary", new Guid("006b1702-3d16-4d64-8d57-9336fbb7c4da")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:school_type:Default:State-funded secondary", new Guid("5c175038-297f-4b0d-89dd-2f6e9e22db29")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:school_type:Default:Special", new Guid("c62fd826-00b0-4933-995c-0739fa7cd1fe")},
                
                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:school_type:Default:State-funded primary", new Guid("514b404f-ca0c-4568-9539-9dea79d43bc8")},
                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:school_type:Default:State-funded secondary", new Guid("df0506d1-456a-4b10-b0a6-268d59c1924e")},
                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:school_type:Default:Special", new Guid("23896559-d8cc-4c0d-b839-1a60d43e5928")},
                
                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:school_type:Default:Total", new Guid("26f7f3d0-b7ad-4517-8815-e0c70295ff3b")},
                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:school_type:Default:State-funded secondary", new Guid("442710bd-5fc6-4096-a4ae-05cb092ede15")},
                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:school_type:Default:State-funded primary", new Guid("2f219985-8900-4a84-b096-89d133ef8bc6")},
                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:school_type:Default:Special", new Guid("d2cbfaa7-9768-4845-9d86-48e352635754")},

                {"faf2152e-0a6c-4e97-af02-e9a89d48c47a:school_type:Default:Total", new Guid("0089b1b3-626c-4326-a2ec-a4b99b4735bc")},
                {"faf2152e-0a6c-4e97-af02-e9a89d48c47a:school_type:Default:State-funded secondary", new Guid("d2b2e7af-d7b7-4526-817c-cc4fc1d010c3")},
                {"faf2152e-0a6c-4e97-af02-e9a89d48c47a:school_type:Default:Special", new Guid("4bd075b2-d4de-48db-a007-dc2f9226d206")},
                
                {"666cd878-87bb-4f77-9a3f-f5c75078e112:school_type:Default:Total", new Guid("0af225c6-c70b-4053-b7e7-4e719e2b751f")},
                {"666cd878-87bb-4f77-9a3f-f5c75078e112:school_type:Default:State-funded secondary", new Guid("c306ff42-ddea-4cd0-82af-770df078fd94")},
                {"666cd878-87bb-4f77-9a3f-f5c75078e112:school_type:Default:Special", new Guid("a7a7a691-a49e-422e-839e-53f1f545fa76")},
                
                {"8e3d1bc0-2beb-4dc6-9db7-3d27d0608042:characteristic:Total:Total", new Guid("beeaa217-3233-48df-bc1d-11f066a26efe")},
                {"8e3d1bc0-2beb-4dc6-9db7-3d27d0608042:characteristic:Gender:Male", new Guid("2cf47ea3-1891-4bba-9381-81a0305a7581")},
                {"8e3d1bc0-2beb-4dc6-9db7-3d27d0608042:characteristic:Gender:Female", new Guid("edcc7822-d88e-490d-8446-baca8b6ccca4")},
                
                {"fa0d7f1d-d181-43fb-955b-fc327da86f2c:nc_year_admission:Primary:R", new Guid("f5ad9114-14b8-4102-89a1-3ab76801ecde")},
                {"fa0d7f1d-d181-43fb-955b-fc327da86f2c:nc_year_admission:Secondary:9", new Guid("3f101896-1c4a-4153-bb22-1d3888eb61ea")},
                {"fa0d7f1d-d181-43fb-955b-fc327da86f2c:nc_year_admission:Secondary:7", new Guid("ff8614ba-ec1c-4012-a5e3-2d788a4f5460")},
            };
        
        private static readonly Dictionary<string, Guid> IndicatorGuids =
            new Dictionary<string, Guid>
            {
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:enrolments", new Guid("1293b484-93f5-4177-a451-fcf4467b26a2")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:sess_possible", new Guid("74c8ab3a-0ec3-4348-852c-9055f354f86f")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:sess_unauthorised_percent", new Guid("ccfe716a-6976-4dc3-8fde-a026cd30f3ae")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:sess_overall_percent", new Guid("92d3437a-0a62-4cd7-8dfb-bcceba7eef61")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:sess_authorised_percent", new Guid("f9ae4976-7cd3-4718-834a-09349b6eb377")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:enrolments_pa_10_exact_percent", new Guid("a93b664a-c537-4bb0-8d06-b4ce9bf60ff9")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence fields:enrolments_pa_10_exact", new Guid("5a59cfeb-19e7-486c-906e-f2ad45f896f6")},
             
                {"803fbf56-600f-490f-8409-6413a891720d:Absence for persistent absentees:sess_authorised_pa_10_exact", new Guid("71c77c7d-dec5-4c31-baa5-b59f3c8c8f2e")},
                
                {"803fbf56-600f-490f-8409-6413a891720d:Absence by reason:sess_unauth_totalreasons", new Guid("a3b1afa4-b3de-44c6-b8b2-0ef59f211a2a")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence by reason:sess_unauth_other", new Guid("71761e61-6a80-400f-8778-a8761306eb77")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence by reason:sess_unauth_noyet", new Guid("b4cc7de4-30f3-495b-a967-0d2a9473583f")},
                {"803fbf56-600f-490f-8409-6413a891720d:Absence by reason:sess_unauth_late", new Guid("57452cb9-6bda-495a-a012-7fda71e4bf90")},
                
                {"568576e5-d386-450e-a8db-307b7061d0d8:Absence fields:sess_overall_percent", new Guid("c5358a0e-50be-4de9-9a7a-366b96c21d2a")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:Absence fields:sess_authorised_percent", new Guid("af786942-5ddc-4e41-8f98-61ca95931985")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:Absence fields:sess_unauthorised_percent", new Guid("2d08d922-d57e-404d-971c-18fb386d3183")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:Absence for persistent absentees:sess_authorised_percent_pa_10_exact", new Guid("1c3cb6ab-1917-448d-b67c-6b7d8438b7ce")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:Absence by reason:sess_auth_illness", new Guid("94f73d1c-dc74-4cae-a2c0-1534c634a4ef")},
                {"568576e5-d386-450e-a8db-307b7061d0d8:Absence by reason:sess_auth_ext_holiday", new Guid("1aa8b098-356d-4b19-942d-e6bc0fded7d8")},

                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:Absence fields:sess_possible", new Guid("cb1fc409-b8fd-482a-aee0-627860cde918")},
                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:Absence fields:sess_unauthorised", new Guid("1199a5e5-eed7-4261-98b9-0a3727104176")},
                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:Absence fields:sess_authorised", new Guid("7a690779-08f3-40c3-80a4-ef5ab1fc0995")},
                {"b7bc537b-0c04-4b15-9eb6-4f0e8cc2e70a:Absence fields:sess_overall", new Guid("a648c8b6-268e-4781-8fb7-801426a270ac")},

                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:Absence fields:enrolments", new Guid("f48c43ca-e6a2-4f16-a5ee-85899536f0a7")},
                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:Absence fields:sess_overall_percent", new Guid("d7113560-e535-421e-9a5b-b6cd7528f4d4")},
                {"353db5ea-befd-488b-ad16-2ce7963c9bc9:Absence fields:sess_overall", new Guid("2550def1-9732-458e-812d-c70d034ec51d")},

                {"95c7f584-907e-4756-bbf0-4905ceae57df:Absence fields:num_schools", new Guid("9472758e-6c4a-4cb2-8270-d0551ce91494")},
                {"95c7f584-907e-4756-bbf0-4905ceae57df:Absence fields:sess_unauthorised_percent", new Guid("6d2de8c1-15d7-4cc4-a60a-d512b7876e4d")},
                {"95c7f584-907e-4756-bbf0-4905ceae57df:Absence fields:sess_authorised_percent", new Guid("6160c4f8-4c9f-40f0-a623-2a4f742860af")},
                {"95c7f584-907e-4756-bbf0-4905ceae57df:Absence fields:sess_overall_percent", new Guid("ee11e1cb-2d9a-4d6d-8e6c-8d32f24fa08f")},
                
                {"faf2152e-0a6c-4e97-af02-e9a89d48c47a:Absence fields:enrol_unauthorised", new Guid("a07cb02d-a07b-4928-a93a-fe1192011998")},
                {"faf2152e-0a6c-4e97-af02-e9a89d48c47a:Absence fields:enrol_authorised", new Guid("037e2587-2c2c-4bec-83e5-12e3cc35c836")},
                {"faf2152e-0a6c-4e97-af02-e9a89d48c47a:Absence fields:enrol_overall", new Guid("ff1ccb6e-5faa-43bc-bd6d-be52f754bde6")},

                {"666cd878-87bb-4f77-9a3f-f5c75078e112:Enrolments by absence percentage band:enrolments_overall", new Guid("fe313349-0438-41b7-8944-109690ee5158")},
                {"666cd878-87bb-4f77-9a3f-f5c75078e112:Enrolments by absence percentage band:enrolments_authorised", new Guid("f3014e60-534a-4667-b90f-80b1fee6b08e")},
                {"666cd878-87bb-4f77-9a3f-f5c75078e112:Enrolments by absence percentage band:enrolments_unauthorised", new Guid("cd2711ff-3dba-4452-858a-d55c5cfd04fb")},
                
                {"fa0d7f1d-d181-43fb-955b-fc327da86f2c:Applications:applications_received", new Guid("020a4da6-1111-443d-af80-3a425c558d14")},
                {"fa0d7f1d-d181-43fb-955b-fc327da86f2c:Applications:online_applications", new Guid("f472e6cc-9e25-401b-9fca-9dc3755bab2d")},
                {"fa0d7f1d-d181-43fb-955b-fc327da86f2c:Applications:online_apps_percent", new Guid("0af5ea39-828f-4afe-9a9f-643dce0112cf")},
            };
        public static void GenerateIndicatorGuids(StatisticsDbContext dbContext)
        {
            foreach (var indicator in dbContext.Indicator.Local.ToList())
            {
                var key = $"{indicator.IndicatorGroup.SubjectId}:{indicator.IndicatorGroup.Label}:{indicator.Name}";
                var id = GetGuid(IndicatorGuids, key);
                if (id != Guid.Empty)
                {
                    indicator.Id = id;
                }
            }
        }
        
        public static void GenerateFilterItemGuids(StatisticsDbContext dbContext)
        {
            foreach (var filterItem in dbContext.FilterItem.Local.ToList())
            {
                var key = $"{filterItem.FilterGroup.Filter.SubjectId}:{filterItem.FilterGroup.Filter.Name}:{filterItem.FilterGroup.Label}:{filterItem.Label}";
                var id = GetGuid(FilterItemGuids, key);
                if (id != Guid.Empty)
                {
                    filterItem.Id = id;
                }
            }
        }

        private static Guid GetGuid(Dictionary<string, Guid> d, string key)
        {
            d.TryGetValue(key, out Guid id);
            return id;
        }
    }
}