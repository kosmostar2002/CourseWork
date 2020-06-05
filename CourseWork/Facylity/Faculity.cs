using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace Facylity_
{
  
    // Клас для органіції факультету
    public class Facylity
    {
        private string _facylityName;
        private List<Speciality> specialities = new List<Speciality>();
        internal int[] auditories_lecture;
        internal int[] auditorias_labs;
        internal Teacher[] Teachers;
        private  TimeTable table;

        // Конструктор, у який подається інформація про назву факультету, номерів аудиторій та викладачів
        // Також створюється пустий загальний розклад
        public Facylity(string facylityName, int[] aud_lec, int[] aud_lab, Teacher[] teachers)
        {
            _facylityName = facylityName;
            auditories_lecture = (int[])aud_lec.Clone();
            auditorias_labs = (int[])aud_lab.Clone();
            Teachers = (Teacher[])teachers.Clone();
            table = new TimeTable(auditories_lecture, auditorias_labs, teachers);
        }
        
        // Функція для створення ноаих спеціальностей на факультеті
        public void AddSpeciality(string specName)
        {
            specialities.Add(new Speciality(specName));
        }

        // Функція, яка викликає функцію спеціальності для створення нового потоку
        public void addFlow(string specName, string flowName, int course)
        {
            int index = specialities.FindIndex(spec => spec.SpecialityName_ == specName);
            specialities[index].AddFlow(flowName, course);
        }

        // Функція, яка викликає функцію потоку для створення нової групи
        public void addGroup(string specName, string flowName, int groupName, int course, string[] students)
        {
            int index1 = specialities.FindIndex(spec => spec.SpecialityName_ == specName);
            int index2 = specialities[index1].Flows.FindIndex(flow => flow._flowName == flowName && 
                                                              flow.Course == course);
            specialities[index1].Flows[index2].AddGroup(groupName, students);
        }

        // Функція для додавання студента в групу
        public void AddStudent(string specName, string flowName, int groupName, int course, string student)
        {
            int index1 = specialities.FindIndex(spec => spec.SpecialityName_ == specName);
            int index2 = specialities[index1].Flows.FindIndex(flow => flow._flowName == flowName &&
                                                              flow.Course == course);
            int index3 = specialities[index1].Flows[index2].groups.FindIndex(gr => gr.groupNumber_ == groupName);
            specialities[index1].Flows[index2].groups[index3].AddStudent(student);
        }

        // Функціяя для видалення студента з групи
        public void RemoveStudent(string specName, string flowName, int groupName, int course, string student)
        {
           
            int index1 = specialities.FindIndex(spec => spec.SpecialityName_ == specName);
            int index2 = specialities[index1].Flows.FindIndex(flow => flow._flowName == flowName &&
                                                              flow.Course == course);
            int index3 = specialities[index1].Flows[index2].groups.FindIndex(gr => gr.groupNumber_ == groupName);
            if (specialities[index1].Flows[index2].groups[index3].listOfStudents.Contains(student))
                specialities[index1].Flows[index2].groups[index3].RemoveStudent(student);
            else
                throw new System.InvalidOperationException("Студента з таким ім'ям не існує");
        }

        // Функція, яка викликає функцію потоку для задання списку предметів
        public void setSubjects(string specName, string flowName, int course,
            string[] subjects, int[] lec, int[] lab)
        {
            int index1 = specialities.FindIndex(spec => spec.SpecialityName_ == specName);
            int index2 = specialities[index1].Flows.FindIndex(flow => flow._flowName == flowName &&
                                                              flow.Course == course);
            for (int i = 0; i < subjects.Length; i++)
                specialities[index1].Flows[index2].AddSubject(subjects[i], lec[i], lab[i]);
        }

        // Функція яка повертає загальне число студентів, які навчаються на факультеті
        public int AllStudents()
        {
            int Res = 0;
            foreach (Speciality spec in specialities)
            {
                foreach (Flow flow in spec.Flows)
                {
                    foreach (Group group in flow.groups)
                    {
                        Res += group.listOfStudents.Count();
                    }
                }
            }
            return Res;
        }

        // Функція, яка складає розклад для відповідної групи
        public void setShedule(string specName, string flowName, int course)
        {
            int index1 = specialities.FindIndex(spec => spec.SpecialityName_ == specName);
            int index2 = specialities[index1].Flows.FindIndex(flow => flow._flowName == flowName &&
                                                              flow.Course == course);
            table.GenTable(specialities[index1].Flows[index2].subjects,
                 specialities[index1].Flows[index2]);
            
        }

        // Функція яка повертає інформацію про пару в даний час для відповідної групи
        public string GetTable(string flow, int group, string day, int pair)
        {
            string Res = "";
            if(table.table[day][pair].groups.Contains((flow, group))){
                Pair para = table.table[day][pair].Pairs.Find(x => x.Group.Any(y => y == (flow, group)));
                Res = para.Subject + " " + para.Type + " Aud: " + para.Auditory + " Teacher: " + para.Teacher;
            }
            return Res;
            
        }
    }
        

    // Структура, яка містить інформацію про викладача
    public struct Teacher
    {
        internal string Name;
        internal string Subject;
        internal int Hours;
        internal List<(string, int)> Lessons;
        public Teacher(string name, string subject, int hours)
        {
            Lessons = new List<(string, int)>();
            Name = name;
            Subject = subject;
            Hours = hours;
        }
    }
}