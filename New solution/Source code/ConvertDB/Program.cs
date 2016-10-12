using ConvertDB.MySQLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace ConvertDB
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var mySQL = new MySQLEntities())
            using (var dbSQLServer = new SQLServerEntities())
            {
                //Console.WriteLine("Convert Patients");
                int count = 0;
                //List<wv_hs_patient> patients = null;
                //while ((patients != null && patients.Count > 0) || count == 0)
                //{
                //    patients = mySQL.Patients.Where(p=>p.id.CompareTo("YICS-0000026119") >0).OrderBy(p => p.id).Skip(count).Take(1000).ToList();
                //    if(patients!=null && patients.Count>0)
                //        Console.WriteLine(count + " >> " + patients[0].id + " " + patients[0].email + " " + patients[0].address);
                //    foreach (var p in patients)
                //    {
                //        //Console.WriteLine(count+" >> "+ p.id + " " + p.email + " " + p.address);
                //        dbSQLServer.Patients.Add(new Patient()
                //        {
                //            Address = p.address,
                //            Birthday = p.dob,
                //            Email = p.email,
                //            Gender = p.gender,
                //            HomePhone = p.h_phone,
                //            Id = Guid.NewGuid(),
                //            LastUpdated = p.last_updated,
                //            MobilePhone = p.m_phone,
                //            MRN = p.id,
                //            Name = p.name,
                //            Pass = p.pass
                //        });
                //        count++;
                //    }
                //    if (patients != null && patients.Count > 0)
                //        Console.WriteLine(count + " >> " + patients[patients.Count-1].id + " " + patients[patients.Count - 1].email + " " + patients[patients.Count - 1].address);
                //    Console.WriteLine("SaveChanges ...");
                //    dbSQLServer.SaveChanges();
                //    //Console.Clear();
                //}
                //Console.WriteLine("Total " + count + " patients");
                //Console.WriteLine("============================================================");

                Console.WriteLine("Convert data");
                count = 0;

                List<wv_hs_patient_data> data = null;
                while ((data != null && data.Count > 0) || count == 0)
                {
                    data = mySQL.Datas.Where(d=>d.id_patient.CompareTo("YICS-0000000201") >=0 //&& 
                                                //d.id_patient.CompareTo("YICS-0000000500") <= 0
                                                )
                                    .OrderBy(d=>d.id_patient).Skip(count).Take(500).ToList();
                    if (data != null && data.Count > 0)
                        Console.WriteLine(count + " >> " + data[0].type + " " + data[0].id_patient + " " + data[0].visit_number);
                    foreach (var item in data)
                    {
                        //Console.WriteLine(count + " >> " + item.type + " " + item.id_patient + " " + item.visit_number);
                        var patient = dbSQLServer.Patients.FirstOrDefault(p => p.MRN == item.id_patient);
                        string type = item.type.Split(new char[] { '_' })[0];
                        JObject json;
                        JArray jsonArr;
                        switch (type)
                        {
                            case "imaging":
                                json = JObject.Parse(item.data);
                                dbSQLServer.ReportDatas.Add(new ReportData()
                                {
                                    CreatedDate = DateTime.ParseExact(Convert.ToString(json["CreatedDate"]), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                    FirstName = Convert.ToString(json["FirstName"]).Trim(),
                                    Id = Guid.NewGuid(),
                                    ID_Old = Convert.ToInt32(json["ID"]),
                                    ImpressionConclusion = Convert.ToString(json["ImpressionConclusion"]),
                                    LastName = Convert.ToString(json["LastName"]).Trim(),
                                    MRN = item.id_patient.Trim(),
                                    PatientId = patient.Id,
                                    ProcedureDescription = Convert.ToString(json["ProcedureDescription"]),
                                    Report = Convert.ToString(json["Report"]),
                                    TestID = Convert.ToInt32(json["TestID"]),
                                    TestName = Convert.ToString(json["TestName"]),
                                    Type = type,
                                    VisitNumber = item.visit_number.Trim()
                                });
                                break;
                            case "report":
                                json = JObject.Parse(item.data);
                                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                dt = dt.AddSeconds(Convert.ToDouble(json["date_created"])).ToLocalTime();
                                dbSQLServer.ReportDatas.Add(new ReportData()
                                {
                                    CreatedDate = dt,
                                    //FirstName = Convert.ToString(json["FirstName"]).Trim(),
                                    Id = Guid.NewGuid(),
                                    ID_Old = Convert.ToInt32(json["ID"]),
                                    //ImpressionConclusion = Convert.ToString(json["ImpressionConclusion"]),
                                    //LastName = Convert.ToString(json["LastName"]).Trim(),
                                    MRN = item.id_patient.Trim(),
                                    PatientId = patient.Id,
                                    //ProcedureDescription = Convert.ToString(json["ProcedureDescription"]),
                                    //Report = Convert.ToString(json["Report"]),
                                    //TestID = Convert.ToInt32(json["TestID"]),
                                    //TestName = Convert.ToString(json["TestName"]),
                                    Type = type,
                                    VisitNumber = item.visit_number.Trim(),
                                    Title = Convert.ToString(json["title"]).Trim(),
                                    Content = Convert.ToString(json["content"]).Trim(),
                                });
                                break;
                            case "prescription":
                                jsonArr = JArray.Parse(item.data);
                                ReportData rnew = new ReportData()
                                {
                                    //CreatedDate = DateTime.ParseExact(Convert.ToString(json["CreatedDate"]), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                    //FirstName = Convert.ToString(json["FirstName"]).Trim(),
                                    Id = Guid.NewGuid(),
                                    //ID_Old = Convert.ToInt32(json["ID"]),
                                    //ImpressionConclusion = Convert.ToString(json["ImpressionConclusion"]),
                                    //LastName = Convert.ToString(json["LastName"]).Trim(),
                                    MRN = item.id_patient.Trim(),
                                    PatientId = patient.Id,
                                    //ProcedureDescription = Convert.ToString(json["ProcedureDescription"]),
                                    //Report = Convert.ToString(json["Report"]),
                                    //TestID = Convert.ToInt32(json["TestID"]),
                                    //TestName = Convert.ToString(json["TestName"]),
                                    Type = type,
                                    VisitNumber = item.visit_number.Trim()
                                };
                                dbSQLServer.ReportDatas.Add(rnew);
                                for (int i = 0; i < jsonArr.Count; i++)
                                {
                                    dbSQLServer.Prescriptions.Add(new Prescription()
                                    {
                                        Cautionary = Convert.ToString(jsonArr[i]["Cautionary"]),
                                        CreatedDate = DateTime.ParseExact(Convert.ToString(jsonArr[0]["CreatedDate"]), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                        DoctorName = Convert.ToString(jsonArr[i]["DoctorName"]),
                                        Dosage = Convert.ToString(jsonArr[i]["Dosage"]),
                                        DrugCode = Convert.ToString(jsonArr[i]["DrugCode"]),
                                        DrugDescription = Convert.ToString(jsonArr[i]["DrugDescription"]),
                                        Duration = Convert.ToString(jsonArr[i]["Duration"]),
                                        Frequency = Convert.ToString(jsonArr[i]["Frequency"]),
                                        Id = Guid.NewGuid(),
                                        Instruction = Convert.ToString(jsonArr[i]["Instruction"]),
                                        ReportDataId = rnew.Id,
                                        Route = Convert.ToString(jsonArr[i]["Route"]),
                                        SpecialInstruction = Convert.ToString(jsonArr[i]["SpecialInstruction"])
                                    });
                                }
                                break;
                            default:
                                json = JObject.Parse(item.data);
                                dbSQLServer.ReportDatas.Add(new ReportData()
                                {
                                    CreatedDate = DateTime.ParseExact(Convert.ToString(json["CreatedDate"]), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                    FirstName = Convert.ToString(json["FirstName"]).Trim(),
                                    Id = Guid.NewGuid(),
                                    ID_Old = Convert.ToInt32(json["ID"]),
                                    ImpressionConclusion = Convert.ToString(json["ImpressionConclusion"]),
                                    LastName = Convert.ToString(json["LastName"]).Trim(),
                                    MRN = item.id_patient.Trim(),
                                    PatientId = patient.Id,
                                    ProcedureDescription = Convert.ToString(json["ProcedureDescription"]),
                                    Report = Convert.ToString(json["Report"]),
                                    TestID = Convert.ToInt32(json["TestID"]),
                                    TestName = Convert.ToString(json["TestName"]),
                                    Type = type,
                                    VisitNumber = item.visit_number.Trim()
                                });
                                break;
                        }
                        count++;
                        //dbSQLServer.SaveChanges();
                    }
                    if (data != null && data.Count > 0)
                        Console.WriteLine(count + " >> " + data[data.Count-1].type + " " + data[data.Count - 1].id_patient + " " + data[data.Count - 1].visit_number);
                    Console.WriteLine("SaveChanges ...");
                    dbSQLServer.SaveChanges();
                    //Console.Clear();
                }
                Console.WriteLine("Total " + count + " rows of data.");
                Console.ReadLine();
            }

        }
    }
}
