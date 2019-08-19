using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;

namespace StudentExercisesMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Instructors
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.Specialty, i.CohortId, c.Name
                                        FROM Instructor i
                                        JOIN Cohort c ON c.Id = CohortId";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        instructor.Cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        instructors.Add(instructor);
                    }
                    reader.Close();

                    return View(instructors);
                }
            }
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int id)
        {
            Instructor instructor = GetSingleInstructor(id);
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            var cohorts = GetAllCohorts();
            var viewModel = new InstructorCreateEditViewModel(cohorts);
            return View(viewModel);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstructorCreateEditViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                using(SqlConnection conn = Connection)
                {
                    conn.Open();
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Instructor 
                                            (FirstName, LastName, SlackHandle, Specialty, CohortId)
                                            VALUES
                                            (@firstName, @lastName, @slackHandle, @specialty, @cohortId)";
                        cmd.Parameters.AddWithValue("@firstName", model.Instructor.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", model.Instructor.LastName);
                        cmd.Parameters.AddWithValue("@slackHandle", model.Instructor.SlackHandle);
                        cmd.Parameters.AddWithValue("@specialty", model.Instructor.Specialty);
                        cmd.Parameters.AddWithValue("@cohortId", model.Instructor.CohortId);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            Instructor instructor = GetSingleInstructor(id);
            var cohorts = GetAllCohorts();
            var viewModel = new InstructorCreateEditViewModel(instructor, cohorts);
            return View(viewModel);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InstructorCreateEditViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Instructor 
                                            SET
                                                FirstName = @firstName, 
                                                LastName = @lastName, 
                                                SlackHandle = @slackHandle, 
                                                Specialty = @specialty, 
                                                CohortId = @cohortId
                                            WHERE Id = @id";
                        cmd.Parameters.AddWithValue("@firstName", model.Instructor.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", model.Instructor.LastName);
                        cmd.Parameters.AddWithValue("@slackHandle", model.Instructor.SlackHandle);
                        cmd.Parameters.AddWithValue("@specialty", model.Instructor.Specialty);
                        cmd.Parameters.AddWithValue("@cohortId", model.Instructor.CohortId);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            Instructor instructor = GetSingleInstructor(id);
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteInstructor(int id)
        {
            try
            {
                // TODO: Add delete logic here
                using(SqlConnection conn = Connection)
                {
                    conn.Open();
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM StudentExercise
                                                WHERE InstructorId = @id
                                            DELETE FROM Instructor
                                                WHERE Id = @id";
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Instructor GetSingleInstructor(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.Specialty, i.CohortId, c.Name
                                        FROM Instructor i
                                        JOIN Cohort c ON c.Id = CohortId
                                        WHERE i.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Instructor instructor = null;
                    if(reader.Read())
                    {
                        instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        instructor.Cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();

                    return instructor;

                }
            }
        }

        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }
    }
}