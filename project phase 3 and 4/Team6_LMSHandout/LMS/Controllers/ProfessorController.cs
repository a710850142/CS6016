using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {
        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            // Find the class ID based on the given parameters
            var query = from cl in db.Classes
                        join c in db.Courses on cl.Listing equals c.CatalogId
                        where c.Department == subject && c.Number == (uint)num
                              && cl.Season == season && cl.Year == (uint)year
                        select cl.ClassId;

            var classId = query.FirstOrDefault();

            if (classId == 0)
            {
                return Json(new object[0]);
            }

            // Query students enrolled in the class
            var studentsQuery = from e in db.Set<Enrolled>()
                                join s in db.Students on e.Student equals s.UId
                                where e.Class == classId
                                select new
                                {
                                    fname = s.FName,
                                    lname = s.LName,
                                    uid = s.UId,
                                    dob = s.Dob,
                                    grade = e.Grade
                                };

            return Json(studentsQuery.ToArray());
        }

        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            // Query assignments with category filter
            var query = from a in db.Assignments
                        join ac in db.AssignmentCategories on a.Category equals ac.CategoryId
                        join cl in db.Classes on ac.InClass equals cl.ClassId
                        join c in db.Courses on cl.Listing equals c.CatalogId
                        where c.Department == subject && c.Number == (uint)num
                              && cl.Season == season && cl.Year == (uint)year
                              && (category == null || ac.Name == category)
                        select new
                        {
                            aname = a.Name,
                            cname = ac.Name,
                            due = a.Due,
                            submissions = a.Submissions.Count
                        };

            return Json(query.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var query = from ac in db.AssignmentCategories
                        join cl in db.Classes on ac.InClass equals cl.ClassId
                        join c in db.Courses on cl.Listing equals c.CatalogId
                        where c.Department == subject && c.Number == (uint)num
                              && cl.Season == season && cl.Year == (uint)year
                        select new
                        {
                            name = ac.Name,
                            weight = ac.Weight
                        };

            return Json(query.ToArray());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            var classQuery = from cl in db.Classes
                             join c in db.Courses on cl.Listing equals c.CatalogId
                             where c.Department == subject && c.Number == (uint)num
                                   && cl.Season == season && cl.Year == (uint)year
                             select cl.ClassId;

            var classId = classQuery.FirstOrDefault();

            if (classId == 0)
            {
                return Json(new { success = false });
            }

            var existingCategory = db.AssignmentCategories
                .FirstOrDefault(ac => ac.InClass == classId && ac.Name == category);

            if (existingCategory != null)
            {
                return Json(new { success = false });
            }

            var newCategory = new AssignmentCategory
            {
                Name = category,
                Weight = (uint)catweight,
                InClass = classId
            };

            db.AssignmentCategories.Add(newCategory);

            try
            {
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            // Find the class and category IDs
            var query = from cl in db.Classes
                        join c in db.Courses on cl.Listing equals c.CatalogId
                        join ac in db.AssignmentCategories on cl.ClassId equals ac.InClass
                        where c.Department == subject && c.Number == (uint)num
                              && cl.Season == season && cl.Year == (uint)year
                              && ac.Name == category
                        select new { ClassId = cl.ClassId, CategoryId = ac.CategoryId };

            var result = query.FirstOrDefault();

            if (result == null)
            {
                return Json(new { success = false });
            }

            // Create new assignment
            var newAssignment = new Assignment
            {
                Name = asgname,
                Contents = asgcontents,
                Due = asgdue,
                MaxPoints = (uint)asgpoints,
                Category = result.CategoryId
            };

            db.Assignments.Add(newAssignment);

            try
            {
                db.SaveChanges();
                UpdateAllStudentGrades(result.ClassId); // Update grades after new assignment
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var query = from s in db.Submissions
                        join a in db.Assignments on s.Assignment equals a.AssignmentId
                        join ac in db.AssignmentCategories on a.Category equals ac.CategoryId
                        join cl in db.Classes on ac.InClass equals cl.ClassId
                        join c in db.Courses on cl.Listing equals c.CatalogId
                        join st in db.Students on s.Student equals st.UId
                        where c.Department == subject && c.Number == (uint)num
                              && cl.Season == season && cl.Year == (uint)year
                              && ac.Name == category && a.Name == asgname
                        select new
                        {
                            fname = st.FName,
                            lname = st.LName,
                            uid = st.UId,
                            time = s.Time,
                            score = s.Score
                        };

            return Json(query.ToArray());
        }

        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            // Find the submission and associated class ID
            var submission = (from s in db.Submissions
                              join a in db.Assignments on s.Assignment equals a.AssignmentId
                              join ac in db.AssignmentCategories on a.Category equals ac.CategoryId
                              join cl in db.Classes on ac.InClass equals cl.ClassId
                              join c in db.Courses on cl.Listing equals c.CatalogId
                              where c.Department == subject && c.Number == (uint)num
                                    && cl.Season == season && cl.Year == (uint)year
                                    && ac.Name == category && a.Name == asgname
                                    && s.Student == uid
                              select new { Submission = s, ClassId = cl.ClassId }).FirstOrDefault();

            if (submission == null)
            {
                return Json(new { success = false });
            }

            // Update the submission score
            submission.Submission.Score = (uint)score;

            try
            {
                db.SaveChanges();
                // Recalculate and update the student's overall grade for the class
                UpdateStudentGrade(submission.ClassId, uid);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from cl in db.Classes
                        join c in db.Courses on cl.Listing equals c.CatalogId
                        where cl.TaughtBy == uid
                        select new
                        {
                            subject = c.Department,
                            number = c.Number,
                            name = c.Name,
                            season = cl.Season,
                            year = cl.Year
                        };

            return Json(query.ToArray());
        }

        private void UpdateAllStudentGrades(uint classId)
        {
            // Get all enrollments for the class
            var enrollments = db.Set<Enrolled>().Where(e => e.Class == classId).ToList();
            // Update grade for each enrolled student
            foreach (var enrollment in enrollments)
            {
                UpdateStudentGrade(classId, enrollment.Student);
            }
        }

        private void UpdateStudentGrade(uint classId, string studentUid)
        {
            // Get all assignment categories for the class
            var categories = db.AssignmentCategories.Where(ac => ac.InClass == classId).ToList();
            double totalWeightedScore = 0;
            double totalWeight = 0;

            foreach (var category in categories)
            {
                // Get all assignments in this category
                var assignments = db.Assignments.Where(a => a.Category == category.CategoryId).ToList();
                if (!assignments.Any()) continue;

                double categoryScore = 0;
                double categoryMaxScore = 0;

                foreach (var assignment in assignments)
                {
                    // Find the student's submission for this assignment
                    var submission = db.Submissions.FirstOrDefault(s => s.Assignment == assignment.AssignmentId && s.Student == studentUid);
                    categoryScore += submission?.Score ?? 0;
                    categoryMaxScore += assignment.MaxPoints;
                }

                if (categoryMaxScore > 0)
                {
                    // Calculate weighted score for this category
                    double categoryPercentage = categoryScore / categoryMaxScore;
                    totalWeightedScore += categoryPercentage * category.Weight;
                    totalWeight += category.Weight;
                }
            }

            string letterGrade = "--";
            if (totalWeight > 0)
            {
                // Calculate final percentage and convert to letter grade
                double finalPercentage = (totalWeightedScore / totalWeight) * 100;
                letterGrade = ConvertPercentageToLetterGrade(finalPercentage);
            }

            // Update the student's grade in the Enrolled table
            var enrollment = db.Set<Enrolled>().FirstOrDefault(e => e.Class == classId && e.Student == studentUid);
            if (enrollment != null)
            {
                enrollment.Grade = letterGrade;
                db.SaveChanges();
            }
        }

        // Helper method to convert percentage to letter grade
        private string ConvertPercentageToLetterGrade(double percentage)
        {
            if (percentage >= 93) return "A";
            if (percentage >= 90) return "A-";
            if (percentage >= 87) return "B+";
            if (percentage >= 83) return "B";
            if (percentage >= 80) return "B-";
            if (percentage >= 77) return "C+";
            if (percentage >= 73) return "C";
            if (percentage >= 70) return "C-";
            if (percentage >= 67) return "D+";
            if (percentage >= 63) return "D";
            if (percentage >= 60) return "D-";
            return "E";
        }
    }
}