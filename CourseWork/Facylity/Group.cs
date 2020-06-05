using System;
using System.Collections.Generic;
using System.Text;

namespace Facylity_
{
    // Клас що відповідає групі студентів
    class Group
    {
        internal int Course;
        internal string flowName_;
        internal int groupNumber_;
        internal List<string> listOfStudents;

        public Group(int groupNumber, string flowName, int course, string[] students)
        {
            Course = course;
            flowName_ = flowName;
            groupNumber_ = groupNumber;
            listOfStudents = new List<string>(students);
        }

        // Функція для видалення студента з даної групи
        public void RemoveStudent(string studentName)
        {
            if (listOfStudents.Contains(studentName))
                listOfStudents.Remove(studentName);
        }

        // Функція для додавання студента з даної групи
        public void AddStudent(string name)
        {
            listOfStudents.Add(name);
            listOfStudents.Sort();
        }
    }

}
