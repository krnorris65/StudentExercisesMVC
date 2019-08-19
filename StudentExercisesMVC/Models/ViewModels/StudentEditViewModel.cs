using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public Student Student { get; set; }
        public List<SelectListItem> Cohorts { get; set; }
        //can the ones already assigned be pre-selected?
        public List<SelectListItem> Exercises { get; set; }
        public List<SelectListItem> Instructors { get; set; }

        public List<int> AssignedExercises { get; set; }
        public int InstructorId { get; set; }

        //Modify your student edit form to display all exercises in a multi-select element.The user should be able to select one, or more exercises, in that element. When the user submits the form, then the StudentExercises table in your database should have a new entry added for each of the exercises that were selected in the form.
        //Users should only be able to select an Instructor that is assigned to that cohort as the Instructor who assigned the exercise

        public StudentEditViewModel() { }
        public StudentEditViewModel(Student student, List<Cohort> cohortList)
        {
            Student = student;
            //use cohort data to create a list of select items
            var selectItems = cohortList
                .Select(cohort => new SelectListItem
                {
                    Text = cohort.Name,
                    Value = cohort.Id.ToString()
                })
                .ToList();

            selectItems.Insert(0, new SelectListItem
            {
                Text = "Choose cohort...",
                Value = "0"
            });

            //Cohorts is a List<SelectListItem>
            Cohorts = selectItems;
        }

        public StudentEditViewModel(Student student, List<Cohort> cohortList, List<Instructor> instructorList, List<Exercise> exerciseList)
        {
            Student = student;
            //use cohort data to create a list of select items
            Cohorts = cohortList
                .Select(cohort => new SelectListItem
                {
                    Text = cohort.Name,
                    Value = cohort.Id.ToString()
                })
                .ToList();

            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose cohort...",
                Value = "0"
            });

            //if there are no instructors for that student's cohort, then exercises shouldn't be able to be assigned to that student
            if(instructorList.Count > 0)
            {
                Instructors = instructorList
                    .Select(i => new SelectListItem
                    {
                        Text = i.FullName,
                        Value = i.Id.ToString()
                    })
                    .ToList();

                Exercises = exerciseList
                    .Select(e => new SelectListItem
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()
                    })
                    .ToList();
            }
        }
    }
}
