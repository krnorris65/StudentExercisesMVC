using StudentExercisesMVC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentInstructorViewModel
    {

        public List<Student> Students { get; set; }
        public List<Instructor> Instructors { get; set; }


        public StudentInstructorViewModel(List<Student> studentList, List<Instructor> instructorList)
        {
            Students = studentList;
            Instructors = instructorList;
        }

    }
}