using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeRepo.Mobile.Models;
using Mobile.Code.Models;

namespace Mobile.Code.Utils
{
    public class ProjectDataStore : IDataStore<Project>
    {
        
        public static ObservableCollection<Project> Projects = new ObservableCollection<Project>()
            {
                new Project
                {
                    Id ="1",
                    ProjectName  = "Sample Project 1 ",
                    Description="This is sample project description.",
                    ProjectImage="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
                    Attendent="Attendent Abhinov",
                    EmployeeName="Point5Nyble",
                    IsStarred=true,
                    CreatedOn=" May 3 ,2020",
                    IdAnimation = $"All{Guid.NewGuid()}",
                    WorkAreas=new ObservableCollection<WorkArea>()
                    {
                        new WorkArea
                        {
                            Details="work Area1", Name="WA1",
                            WorkAreaImages=new ObservableCollection<ItemImage>()
                            {
                                new ItemImage
                                {
                                    ImageDescription="sample floor",
                                    ImageNotes="Sample Image Notes",
                                    ImagePath="https://thumbs.dreamstime.com/z/construction-site-construction-workers-area-people-working-construction-group-people-professional-construction-118630790.jpg",
                                    Name="Floor 1"

                                },
                                new ItemImage
                                {
                                    ImageDescription="sample pool",
                                    ImageNotes="Sample Image Pool notes , this is a big note with a lot of characters . ;ets see/",
                                    ImagePath="https://m.economictimes.com/thumb/msid-69127844,width-1200,height-900,resizemode-4,imgsize-347903/construction-site-generators-types-features-of-generators-used-at-construction-sites.jpg",
                                    Name="Pool 1"

                                }
                            }
                        },
                        new WorkArea
                        {
                            Details="work Area2", Name="WA2",
                            WorkAreaImages=new ObservableCollection<ItemImage>()
                            {
                                new ItemImage
                                {
                                    ImageDescription="sample floor",
                                    ImageNotes="Sample Image Notes hen ten pen shcekla a;sdka le lo bhaat lelo ",
                                    ImagePath="https://www.thenbs.com/-/media/uk/new-images/by-size/1500_hero/helmetcolours_1500.jpg",
                                    Name="Floor 2"

                                },
                                new ItemImage
                                {
                                    ImageDescription="sample pool",
                                    ImageNotes="Sample Image Pool notes , this is a big note with a lot of characters . ;ets see/ loreum ipsum hen ten",
                                    ImagePath="https://www.constructionglobal.com/sites/default/files/styles/og_image/public/bizclik-drupal-prod/topic/image/CCCon_0.jpg?itok=VNCgUkk",
                                    Name="Pool 2"

                                }
                            }
                        }
                    }
                },
                new Project
                {
                    ProjectId = Guid.NewGuid().ToString(),
                    ProjectName  = "Sample Project 2",
                    Description="This is sample project description. a little big description for test",
                    ProjectImage="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
                    Attendent="Attendent Pankaj",
                    EmployeeName="Point5Nyble",
                    CreatedOn=" April 3 ,2019",
                    IdAnimation = $"All{Guid.NewGuid()}",
                    WorkAreas=new ObservableCollection<WorkArea>()
                    {
                        new WorkArea
                        {
                            Details="work Area4", Name="WA4",
                            WorkAreaImages=new ObservableCollection<ItemImage>()
                            {
                                new ItemImage
                                {
                                    ImageDescription="sample ladders",
                                    ImageNotes="Sample Image Notes blah blah clah chal ",
                                    ImagePath="https://upload.wikimedia.org/wikipedia/commons/e/ec/The_crane_and_the_Main_Street_midrise_on_the_Infinity_(300_Spear_Street)_construction_site,_SF.JPG",
                                    Name="ladder 1"

                                },
                                new ItemImage
                                {
                                    ImageDescription="sample pool",
                                    ImageNotes="Sample Image Pool notes , this is a big note with a lot of characters . ;ets see/",
                                    ImagePath="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
                                    Name="Pool blue 232 1"

                                }
                            }
                        },
                        new WorkArea
                        {
                            Details="work Area5", Name="WA5",
                            WorkAreaImages=new ObservableCollection<ItemImage>()
                            {
                                new ItemImage
                                {
                                    ImageDescription="sample room",
                                    ImageNotes="Sample Image Notes hen ten pen shcekla a;sdka le lo bhaat lelo ",
                                    ImagePath="https://www.dolmanlaw.com/wp-content/uploads/2017/07/Construction-Sites-can-Cause-Injuries-to-Non-Workers.jpg",
                                    Name="room 2"

                                },
                                new ItemImage
                                {
                                    ImageDescription="sample kitchen",
                                    ImageNotes="Sample Image Pool notes , this is a big note with a lot of characters . ;ets see/ loreum ipsum hen ten",
                                    ImagePath="https://www.cambridgenetwork.co.uk/sites/default/files/__Construction_1.jpg",
                                    Name="Kitchen 2"

                                }
                            }
                        }
                    }
                }

            }; 

        public ProjectDataStore()
        {
            
        }

        public async Task<bool> AddItemAsync(Project item)
        {
            Projects.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Project item)
        {
            var oldItem = Projects.Where(x=>x.ProjectId == item.ProjectId)
                .FirstOrDefault();
            Projects.Remove(oldItem);
            Projects.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = Projects.Where(x=>x.ProjectId == id).FirstOrDefault();
            Projects.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Project> GetItemAsync(string id)
        {
            return await Task.FromResult(Projects.FirstOrDefault(s => s.ProjectId == id));
        }

        public async Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Projects);
        }
    }
}
