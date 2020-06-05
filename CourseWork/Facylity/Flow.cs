using System;
using System.Collections.Generic;
using System.Text;

namespace Facylity_
{
    // Клас що відповідає за потік
    class Flow
    {
        internal string _flowName;
        internal int Course;
        internal List<Group> groups = new List<Group>();

        // Словник містить інформацію про предмет-ключ та кількість годин які на нього відводяться
        // у форматі (Години для лекцій, Години для лаборанторних)
        internal IDictionary<string, (int, int)> subjects = new Dictionary<string, (int, int)>();

        public Flow(string flowName, int course)
        {
            _flowName = flowName;
            Course = course;
        }

        // Функція яка створює групу у потоці, при цьому відсортувавши елементи цього списку
        public void AddGroup(int groupNunmer, string[] students)
        {
            groups.Add(new Group(groupNunmer, this._flowName, this.Course, students));
            groups[groups.Count - 1].listOfStudents = new List<string>(students);
            groups[groups.Count - 1].listOfStudents.Sort();
        }

        // Функція яка додає предмет до програми потоку
        public void AddSubject(string SubjectName, int LectureHours, int LabHours)
        {
            subjects.Add(SubjectName, (LectureHours, LabHours));
        }
    }

}
