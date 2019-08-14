using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class InstructorCreateEditViewModel
    {
        public Instructor Instructor { get; set; }
        public List<SelectListItem> Cohorts { get; set; }

        public InstructorCreateEditViewModel() { }

        //used for Create
        public InstructorCreateEditViewModel(List<Cohort> cohortList)
        {
            CreateListOfSelectListItem(cohortList);
        }

        //used for Edit
        public InstructorCreateEditViewModel(Instructor instructor, List<Cohort> cohortList)
        {
            Instructor = instructor;
            CreateListOfSelectListItem(cohortList);

        }

        //reusable method that creates a list of select items from a list of cohorts and sets the value of Cohorts
        private void CreateListOfSelectListItem(List<Cohort> cohortList)
        {
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


    }
}
