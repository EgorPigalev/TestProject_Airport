using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class PageChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static int countButton = 2;
        static int countitems = 5;
        public int[] NPage { get; set; } = new int[countitems];
        public string[] Visible { get; set; } = new string[countitems];
        public string[] Bold { get; set; } = new string[countitems];
        public string[] VisibleButton { get; set; } = new string[countButton];

        int countpages;
        public int CountPages
        {
            get => countpages;
            set
            {
                countpages = value;
                for (int i = 1; i < countitems; i++)
                {
                    if (CountPages <= i)
                    {
                        Visible[i] = "Hidden";
                    }
                    else
                    {
                        Visible[i] = "Visible";
                    }
                }
            }
        }

        int countpage;
        public int CountPage
        {
            get => countpage;
            set
            {
                countpage = value;
                if(value != 0)
                {
                    if (Countlist % value == 0)
                    {
                        CountPages = Countlist / value;
                    }
                    else
                    {
                        CountPages = Countlist / value + 1;
                    }
                }
                else
                {
                    CountPages = 0;
                }
            }
        }

        int countlist;
        public int Countlist
        {
            get => countlist;
            set
            {
                countlist = value;
                if (value != 0)
                {
                    if (value % CountPage == 0)
                    {
                        CountPages = value / CountPage;
                    }
                    else
                    {
                        CountPages = 1 + value / CountPage;
                    }
                }
                else
                {
                    CountPages = 0;
                }
            }
        }
        int currentpage;
        public int CurrentPage
        {
            get => currentpage;
            set
            {
                currentpage = value;
                if (currentpage < 1)
                {
                    currentpage = 1;
                }
                if (currentpage >= CountPages)
                {
                    currentpage = CountPages;
                }                    
                for (int i = 0; i < countitems; i++)
                {
                    if (currentpage < (1 + countitems / 2) || CountPages < countitems) NPage[i] = i + 1;
                    else if (currentpage > CountPages - (countitems / 2 + 1)) NPage[i] = CountPages - (countitems - 1) + i;
                    else NPage[i] = currentpage + i - (countitems / 2);
                }
                for (int i = 0; i < countitems; i++)
                {
                    if (NPage[i] == currentpage) Bold[i] = "ExtraBold";
                    else Bold[i] = "Regular";
                }

                //Смена видимости для перемещения в самое начало и конец
                if (countpages > 5)
                {
                    if (NPage[0] != 1)
                    {
                        VisibleButton[0] = "Visible";
                    }
                    else
                    {
                        VisibleButton[0] = "Hidden";
                    }
                    if (NPage[4] != countpages)
                    {
                        VisibleButton[1] = "Visible";
                    }
                    else
                    {
                        VisibleButton[1] = "Hidden";
                    }
                }
                else
                {
                    VisibleButton[0] = "Hidden";
                    VisibleButton[1] = "Hidden";
                }

                PropertyChanged(this, new PropertyChangedEventArgs("NPage"));
                PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                PropertyChanged(this, new PropertyChangedEventArgs("Bold"));
                PropertyChanged(this, new PropertyChangedEventArgs("VisibleButton"));
            }
        }

        public PageChange()
        {
            for (int i = 0; i < countitems; i++)
            {
                if (i == 0)
                {
                    Visible[i] = "Visible";
                    Bold[i] = "ExtraBold";
                }
                else
                {
                    Visible[i] = "Hidden";
                    Bold[i] = "Regular";
                }

                NPage[i] = i + 1;

            }
            currentpage = 1;
            countlist = 1;
        }
    }
}
