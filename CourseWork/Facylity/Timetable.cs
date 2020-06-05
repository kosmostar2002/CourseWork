using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Facylity_
{

    // Клас який організовує роботу загального розкладу
    class TimeTable
    {
        private string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        internal IDictionary<string, Lesson[]> table = new Dictionary<string, Lesson[]>();
        private Teacher[] _Teachers;

        public TimeTable(int[] audLec, int[] audLab, Teacher[] teachers)
        {
            _Teachers = teachers;
            List<Lesson> freeDay = new List<Lesson>();
            for (int i = 0; i < 5; i++)
            {
                freeDay.Add(new Lesson(audLec, audLab));
            }

            table.Add("Monday", freeDay.Select(_ => new Lesson(audLec, audLab)).ToArray());
            table.Add("Tuesday", freeDay.Select(_ => new Lesson(audLec, audLab)).ToArray());
            table.Add("Wednesday", freeDay.Select(_ => new Lesson(audLec, audLab)).ToArray());
            table.Add("Thursday", freeDay.Select(_ => new Lesson(audLec, audLab)).ToArray());
            table.Add("Friday", freeDay.Select(_ => new Lesson(audLec, audLab)).ToArray());

        }

        // Функція яка шукає вільну пару для даної групи
        private (string, int) findFreePair(Group curGroup, Flow curFlow, string subject, string type)
        {
            if (type == "Lecture")
                for (int i = 0; i < 5; i++)
                {
                    foreach (string day in days)
                    {
                        bool cond = true;
                        if (table[day][i]._AuditoryLec.Count != 0)
                            foreach(Group gr in curFlow.groups)
                            {
                                if (!(table[day][i].groups.All(x => x != (gr.flowName_, gr.groupNumber_))))
                                    cond = false;              
                            }
                            if (cond) 
                                if (_Teachers.Any(x => x.Subject == subject && x.Hours > 0 &&
                                    !x.Lessons.Contains((day, i))))
                                        return (day, i);
                        
                    }
                }

            else if (type == "Laba")
                for (int i = 0; i < 5; i++)
                {
                    foreach (string day in days)
                    {
                        if (table[day][i]._AuditoryLab.Count != 0)
                            if (table[day][i].groups.All(x => x != (curGroup.flowName_, curGroup.groupNumber_)))
                                if (_Teachers.Any(x => x.Subject == subject && x.Hours > 0 &&
                                        !x.Lessons.Contains((day, i))))
                                    return (day, i);
                    }
                }
            return ("None", 0);
        }

        // Фунція яка складає розклад для вібповідного потоку за його навчальним планом
        public void GenTable(IDictionary<string, (int, int)> subj,   Flow curFlow)
        {
            int lecH, labH, pairNumber, aud;
            string day;
            Teacher teacher;
            foreach (var item in subj)
            {
                (lecH, labH) = item.Value;
                for (int i = 0; i < lecH; i++)
                {
                    (day, pairNumber) = findFreePair(curFlow.groups[0], curFlow,  item.Key, "Lecture");

                    if (day != "None")
                    {
                        table[day][pairNumber].flows.Add(curFlow._flowName);

                        aud = table[day][pairNumber]._AuditoryLec[0];
                        table[day][pairNumber]._AuditoryLec.RemoveAt(0);

                        teacher = _Teachers.FirstOrDefault(x => x.Subject == item.Key && x.Hours > 0 &&
                                                           !x.Lessons.Contains((day, pairNumber)));
                        teacher.Lessons.Add((day, pairNumber));
                        _Teachers[Array.IndexOf(_Teachers, teacher)].Hours -= 1;
                        table[day][pairNumber].Pairs.Add(new Pair(item.Key, aud, curFlow._flowName, "Lecture", teacher.Name));

                        foreach (Group gr in curFlow.groups)
                        {
                            table[day][pairNumber].groups.Add((gr.flowName_, gr.groupNumber_));
                            table[day][pairNumber].Pairs[table[day][pairNumber].Pairs.Count - 1].Group.Add((curFlow._flowName, gr.groupNumber_));
                        }
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Немає вільних пар");
                    }
                }

                for (int i = 0; i < labH; i++)
                {
                    foreach (Group gr in curFlow.groups)
                    {
                        (day, pairNumber) = findFreePair(gr, curFlow, item.Key, "Laba");
                        if (day != "None")
                        {
                            table[day][pairNumber].flows.Add(curFlow._flowName);

                            aud = table[day][pairNumber]._AuditoryLab[0];
                            table[day][pairNumber]._AuditoryLab.RemoveAt(0);

                            teacher = _Teachers.FirstOrDefault(x => x.Subject == item.Key && x.Hours > 0 &&
                                                                !x.Lessons.Contains((day, pairNumber)));
                            teacher.Lessons.Add((day, pairNumber));
                            _Teachers[Array.IndexOf(_Teachers, teacher)].Hours -= 1;
                            table[day][pairNumber].Pairs.Add(new Pair(item.Key, aud, curFlow._flowName, "Laba", teacher.Name));
                            table[day][pairNumber].Pairs[
                                table[day][pairNumber].Pairs.Count - 1].Group.Add((curFlow._flowName, gr.groupNumber_));

                            table[day][pairNumber].groups.Add((curFlow._flowName, gr.groupNumber_));
                        }
                        else
                        {
                            Console.WriteLine(item.Key);
                        }
                    }
                }

            }

        }
    }

   
    //Структура яка відповідає за всі пари в один час
    struct Lesson
    {

        internal List<(string, int)> groups;
        internal List<string> flows;
        internal List<Pair> Pairs;
        internal List<int> _AuditoryLec;
        internal List<int> _AuditoryLab;

        public Lesson(int[] auditoryLec, int[] auditoryLab)
        {
            _AuditoryLec = new List<int>(auditoryLec);
            _AuditoryLab = new List<int>(auditoryLab);
            flows = new List<string>();
            groups = new List<(string, int)>();
            Pairs = new List<Pair>();
        }

    }


    // Структура яка відповідає за якусь пару для конкретної групи
    struct Pair
    {
        internal string Subject;
        internal int Auditory;
        public List<(string, int)> Group;
        internal string Flow;
        internal string Type;
        internal string Teacher;

        public Pair(string subject, int auditory, string flow, string type, string teacher)
        {
            Subject = subject;
            Flow = flow;
            Group = new List<(string, int)>();
            Auditory = auditory;
            Type = type;
            Teacher = teacher;
        }

    }

}
