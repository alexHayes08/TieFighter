using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TieFighter.Areas.Admin.Models.MedalsViewModels;

namespace TieFighter.Models
{
    public class TieFighterDatastoreContext
    {
        #region Constructors

        public TieFighterDatastoreContext(string projectId)
        {
            //string projectId = configuration["Authentication:Google:ProjectId"];
            Db = DatastoreDb.Create(projectId);

            AudioSettingsKeyFactory = Db.CreateKeyFactory(audioSettingsKindName);
            ControlSettingsKeyFactory = Db.CreateKeyFactory(controlSettingsKindName);
            MedalsKeyFactory = Db.CreateKeyFactory(medalKindName);
            ShipsKeyFactory = Db.CreateKeyFactory(shipKindName);
            ToursKeyFactory = Db.CreateKeyFactory(tourKindName);
            MissionsKeyFactory = Db.CreateKeyFactory(missionKindName);
            UsersKeyFactory = Db.CreateKeyFactory(userKindName);
            VideoSettingsKeyFactory = Db.CreateKeyFactory(videoSettingsKindName);

            lastUpdatesToTables = new Dictionary<string, DateTime>
            {
                { nameof(AudioSettings), DateTime.Now },
                { nameof(ControlSettings), DateTime.Now },
                { nameof(Medal), DateTime.Now },
                { nameof(Mission), DateTime.Now },
                { nameof(Ship), DateTime.Now },
                { nameof(Tour), DateTime.Now },
                { nameof(User), DateTime.Now },
                { nameof(VideoSettings), DateTime.Now }
            };

            keyFactories = new Dictionary<string, KeyFactory>
            {
                { audioSettingsKindName, AudioSettingsKeyFactory },
                { controlSettingsKindName, ControlSettingsKeyFactory },
                { medalKindName, MedalsKeyFactory },
                { shipKindName, ShipsKeyFactory },
                { tourKindName, ToursKeyFactory },
                { missionKindName, MissionsKeyFactory },
                { userKindName, UsersKeyFactory },
                { videoSettingsKindName, VideoSettingsKeyFactory }
            };
        }

        #endregion

        #region Fields

        public readonly DatastoreDb Db;
        public readonly KeyFactory AudioSettingsKeyFactory;
        public readonly KeyFactory ControlSettingsKeyFactory;
        public readonly KeyFactory MedalsKeyFactory;
        public readonly KeyFactory MissionsKeyFactory;
        public readonly KeyFactory ShipsKeyFactory;
        public readonly KeyFactory ToursKeyFactory;
        public readonly KeyFactory UsersKeyFactory;
        public readonly KeyFactory VideoSettingsKeyFactory;
        private readonly IDictionary<string, DateTime> lastUpdatesToTables;
        private readonly IDictionary<string, KeyFactory> keyFactories;

        private const string audioSettingsKindName = nameof(AudioSettings);
        private const string controlSettingsKindName = nameof(ControlSettings);
        private const string medalKindName = nameof(Medal);
        private const string missionKindName = nameof(Mission);
        private const string shipKindName = nameof(Ship);
        private const string tourKindName = nameof(Tour);
        private const string userKindName = nameof(User);
        private const string videoSettingsKindName = nameof(VideoSettings);

        #endregion

        #region Properties

        #endregion

        #region Functions
       
        public IList<Medal> GetPaginatedMedals(int pageNumber, int resultsPerPage, DateTime lastUpdated)
        {
            var response = GetPaginatedOf(nameof(Medal), resultsPerPage, pageNumber, lastUpdated);
            return DatastoreHelpers.ParseEntitiesToObject<Medal>(response);
        }

        private IReadOnlyList<Entity> GetPaginatedOf(string kind, int resultsPerPage, int pageNumber, DateTime lastUpdated)
        {
            var query = new Query(kind)
            {
                Limit = resultsPerPage,
                Offset = resultsPerPage * pageNumber
            };
            var response = Db.RunQuery(query).Entities;

            return response;
        }

        public KeyFactory GetKeyFactoryForKind(string kindname)
        {
            foreach (var key in keyFactories.Keys)
            {
                if (key == kindname)
                {
                    return keyFactories[key];
                }
            }

            throw new KeyNotFoundException($"Failed to find a KeyFactory for kind {kindname}.");
        }

        private const string MedalConditionValueName = nameof(MedalCondition.ConditionValue);
        private static void SetMedalEntityValue(MedalCondition condition, ref Entity entity)
        {
            switch (condition.ConditionType)
            {
                case MedalConditionTypes.KillCount:
                    entity[MedalConditionValueName] = (int)condition.ConditionValue;
                    break;
                case MedalConditionTypes.StatAt:
                    var stat = (MedalConditionalTypeStats)condition.ConditionValue;
                    var subEntity = new Entity()
                    {
                        ["Name"] = stat.Name,
                        ["Value"] = stat.Value,
                        ["Operator"] = Enum.GetName(typeof(MedalConditionalTypeStatsOperators), stat.Operator),
                        ["ValueAsPercent"] = stat.ValueAsPercent
                    };
                    entity[MedalConditionValueName] = subEntity;
                    break;
                case MedalConditionTypes.TimeSpan:
                    entity[MedalConditionValueName] = (DateTime)condition.ConditionValue;
                    break;
                case MedalConditionTypes.TotalTravelDistance:
                    entity[MedalConditionValueName]= (double)condition.ConditionValue;
                    break;
                case MedalConditionTypes.WithoutDying:
                    entity[MedalConditionValueName] = null;
                    break;
                default:
                    break;
            }
        }

        #region Seed db with data

        public static async Task InitializeDbAsync(UserManager<ApplicationUser> userManager)
        {
            var dbContext = new TieFighterDatastoreContext("tiefighter-imperialremnant");
            var medalKeyFactory = dbContext.Db.CreateKeyFactory(medalKindName);

            // Upsert medals
            var medalEntities = new List<Entity>();
            var medals = new List<Medal>()
            {
                new Medal()
                {
                    MedalName = "Double Kill",
                    Description = "Kill two enemies within two seconds of each other.",
                    PointsWorth = 2,
                    Conditions = new MedalCondition[]
                    {
                        new MedalCondition()
                        {
                            ConditionType = MedalConditionTypes.KillCount,
                            ConditionValue = 2,
                            DependsOn = new MedalCondition()
                            {
                                ConditionType = MedalConditionTypes.TimeSpan,
                                ConditionValue = new DateTime(1,1,1,0,0,2).ToUniversalTime()
                            },
                        }
                    },
                    FileLocation = "Double Kill.png"
                },
                new Medal()
                {
                    MedalName = "Triple Kill",
                    Description = "Kill three enemies within two seconds of each other.",
                    PointsWorth = 2,
                    Conditions = new MedalCondition[]
                    {
                        new MedalCondition()
                        {
                            ConditionType = MedalConditionTypes.KillCount,
                            ConditionValue = 3,
                            DependsOn = new MedalCondition()
                            {
                                ConditionType = MedalConditionTypes.TimeSpan,
                                ConditionValue = new DateTime(1,1,1,0,0,2).ToUniversalTime()
                            }
                        }
                    },
                    FileLocation = "Triple Kill.png"
                },
                new Medal()
                {
                    MedalName = "Ace",
                    Description = "Get five confirmed kills in one life.",
                    PointsWorth = 5,
                    Conditions = new MedalCondition[]
                    {
                        new MedalCondition()
                        {
                            ConditionType = MedalConditionTypes.KillCount,
                            ConditionValue = 5,
                            DependsOn = new MedalCondition()
                            {
                                ConditionType = MedalConditionTypes.WithoutDying,
                                ConditionValue = null
                            }
                        }
                    },
                    FileLocation = "Ace.png"
                },
                new Medal()
                {
                    MedalName = "Double Ace",
                    Description = "Get ten confirmed kills in one life.",
                    PointsWorth = 10,
                    Conditions = new MedalCondition[]
                    {
                        new MedalCondition()
                        {
                            ConditionType = MedalConditionTypes.KillCount,
                            ConditionValue = 10,
                            DependsOn = new MedalCondition()
                            {
                                ConditionType = MedalConditionTypes.WithoutDying,
                                ConditionValue = null
                            }
                        }
                    },
                    FileLocation = "DoubleAce.png"
                },
                new Medal()
                {
                    MedalName = "Survivalist",
                    Description = "Survive for 2 minutes when hull integrity is less than 10%.",
                    PointsWorth = 4,
                    Conditions = new MedalCondition[]
                    {
                        new MedalCondition()
                        {
                            ConditionType = MedalConditionTypes.StatAt,
                            ConditionValue = new MedalConditionalTypeStats()
                            {
                                Name = "HullIntegrity",
                                Value = 10,
                                ValueAsPercent = true,
                                Operator = MedalConditionalTypeStatsOperators.LessThanOrEqual
                            },
                            DependsOn = new MedalCondition()
                            {
                                ConditionType = MedalConditionTypes.TimeSpan,
                                ConditionValue = new DateTime(1,1,1,0,2,0).ToUniversalTime(),
                                DependsOn = new MedalCondition()
                                {
                                    ConditionType = MedalConditionTypes.WithoutDying,
                                    ConditionValue = null,
                                }
                            }
                        }
                    },
                    FileLocation = ""
                }
            };

            foreach (var medal in medals)
            {
                var medalConditions = new List<Entity>();
                foreach (var condition in medal.Conditions)
                {
                    var entity = new Entity();
                    entity["ConditionType"] = Enum.GetName(typeof(MedalConditionTypes), condition.ConditionType);
                    SetMedalEntityValue(condition, ref entity);

                    Entity dependsOnEntity = null;
                    for (var nestedCondition = condition;
                        nestedCondition.DependsOn != null; 
                        nestedCondition = nestedCondition.DependsOn)
                    {
                        var newEntity = new Entity()
                        {
                            ["ConditionType"] = Enum.GetName(typeof(MedalConditionTypes), condition.ConditionType),
                        };
                        SetMedalEntityValue(nestedCondition.DependsOn, ref newEntity);

                        if (dependsOnEntity != null)
                        {
                            dependsOnEntity["DependsOn"] = newEntity;
                        }
                        else
                        {
                            entity["DependsOn"] = newEntity;
                        }

                        dependsOnEntity = newEntity;
                    }

                    medalConditions.Add(entity);
                }

                medalEntities.Add(new Entity()
                {
                    Key = medalKeyFactory.CreateKey(medal.MedalName),
                    ["MedalName"] = new Value()
                    {
                        StringValue = medal.MedalName
                    },
                    ["Description"] = new Value()
                    {
                        StringValue = medal.Description,
                        ExcludeFromIndexes = true
                    },
                    ["PointsWorth"] = new Value()
                    {
                        DoubleValue = medal.PointsWorth,
                        ExcludeFromIndexes = true
                    },
                    ["Conditions"] = new Value()
                    {
                        ArrayValue = medalConditions.ToArray()
                    },
                    ["FileLocation"] = new Value()
                    {
                        StringValue = medal.FileLocation
                    }
                });
            }

            await dbContext.Db.UpsertAsync(medalEntities);

            // Upsert Ships
            var shipKeyFactory = dbContext.Db.CreateKeyFactory(shipKindName);

            var shipEntities = new List<Entity>();
            var ships = new List<Ship>()
            {
                new Ship()
                {
                    FileLocation = "TieFighter.babylon",
                    DisplayName = "TIE-Fighter"
                },
                new Ship()
                {
                    FileLocation = "TieBomber.babylon",
                    DisplayName = "TIE-Bomber"
                },
                new Ship()
                {
                    FileLocation = "TieIntercepter.babylon",
                    DisplayName = "TIE-Intercepter"
                },
                new Ship()
                {
                    FileLocation = "StarDestroyerMark1.babylon",
                    DisplayName = "Imperial I - Star Destroyer"
                },
                new Ship()
                {
                    FileLocation = "XWing.babylon",
                    DisplayName = "X-Wing"
                },
                new Ship()
                {
                    FileLocation = "YWing.babylon",
                    DisplayName = "Y-Wing"
                },
                new Ship()
                {
                    FileLocation = "AWing.babylon",
                    DisplayName = "A-Wing"
                }
            };

            foreach (var ship in ships)
            {
                shipEntities.Add(new Entity()
                {
                    Key = shipKeyFactory.CreateKey(ship.DisplayName.Replace(" ", "_")),
                    ["FileLocation"] = new Value()
                    {
                        StringValue = ship.FileLocation,
                        ExcludeFromIndexes = true
                    },
                    ["DisplayName"] = new Value()
                    {
                        StringValue = ship.DisplayName,
                    }
                });
            }

            await dbContext.Db.UpsertAsync(shipEntities);

            // Upsert tours/missions
            var tourKeyFactory = dbContext.Db.CreateKeyFactory(tourKindName);
            var tourEntities = new List<Entity>();
            var missionEntities = new List<Entity>();
            var tours = new List<Tour>()
            {
                new Tour()
                {
                    TourName = "Tour of Duty I: Aftermath of Hoth",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Patrol Jump Point D-34",
                        },
                        new Mission()
                        {
                            DisplayName = "Red Alert",
                        },
                        new Mission()
                        {
                            DisplayName = "Counter-Attack",
                        },
                        new Mission()
                        {
                            DisplayName = "Outpost D-34 Has Fallen"
                        },
                        new Mission()
                        {
                            DisplayName = "Attack Rebel Lt.Cruiser"
                        },
                        new Mission()
                        {
                            DisplayName = "Destroy the Lulsla"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty II: The Sepan Civil War",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Respond To S.O.S."
                        },
                        new Mission()
                        {
                            DisplayName = "Intercept Attack"
                        },
                        new Mission()
                        {
                            DisplayName = "Rescue War Refugees"
                        },
                        new Mission()
                        {
                            DisplayName = "Capture Enemies"
                        },
                        new Mission()
                        {
                            DisplayName = "Guard Resupply"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty III: Battle on the Frontier",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Load Base Equipment"
                        },
                        new Mission()
                        {
                            DisplayName = "Destroy Pirate Outpost"
                        },
                        new Mission()
                        {
                            DisplayName = "Hold Position"
                        },
                        new Mission()
                        {
                            DisplayName = "Guard Space Station NL-1"
                        },
                        new Mission()
                        {
                            DisplayName = "Thrawn Inspects NL-1"
                        },
                        new Mission()
                        {
                            DisplayName = "Wait for Relief Forces"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty IV: Conflict at Mylock IV",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Escort Convoy"
                        },
                        new Mission()
                        {
                            DisplayName = "Attack the Nharwaak"
                        },
                        new Mission()
                        {
                            DisplayName = "Defend Tech Center"
                        },
                        new Mission()
                        {
                            DisplayName = "Diplomatic Meeting"
                        },
                        new Mission()
                        {
                            DisplayName = "Rebel Arms Deal"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty V: Battle for Honor",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Mine Clearing"
                        },
                        new Mission()
                        {
                            DisplayName = "Assault Gunboat Recon"
                        },
                        new Mission()
                        {
                            DisplayName = "Convoy Attack"
                        },
                        new Mission()
                        {
                            DisplayName = "Tactical Superiority"
                        },
                        new Mission()
                        {
                            DisplayName = "Capture Harkov"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty VI: Arms Race",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Protect Prototypes"
                        },
                        new Mission()
                        {
                            DisplayName = "Prevent Rebel Ambush"
                        },
                        new Mission()
                        {
                            DisplayName = "Convoy Escort"
                        },
                        new Mission()
                        {
                            DisplayName = "Punitive Raid"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty VII: Treachery at Ottega",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Trap the Protector"
                        },
                        new Mission()
                        {
                            DisplayName = "Destroy the Akaga"
                        },
                        new Mission()
                        {
                            DisplayName = "Retribution"
                        },
                        new Mission()
                        {
                            DisplayName = "TIE Defender"
                        },
                        new Mission()
                        {
                            DisplayName = "Save the Emperor"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty VIII: Strategic Warfare",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Evacuate TIE Avenger Plants"
                        },
                        new Mission()
                        {
                            DisplayName = "Save TIE Avenger Factory"
                        },
                        new Mission()
                        {
                            DisplayName = "Secure TIE Avenger Plant"
                        },
                        new Mission()
                        {
                            DisplayName = "Capture Mag Pulse Weapon"
                        },
                        new Mission()
                        {
                            DisplayName = "Trapped"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty IX: T/D Technology",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Capture the Platform"
                        },
                        new Mission()
                        {
                            DisplayName = "Hold Platform"
                        },
                        new Mission()
                        {
                            DisplayName = "Protect Evacuation"
                        },
                        new Mission()
                        {
                            DisplayName = "Escort to Rendezvous"
                        },
                        new Mission()
                        {
                            DisplayName = "Trapped by Pirates"
                        },
                        new Mission()
                        {
                            DisplayName = "Transfer Prototypes"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty X: New Threats",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Ransom"
                        },
                        new Mission()
                        {
                            DisplayName = "Rescue"
                        },
                        new Mission()
                        {
                            DisplayName = "Under the Gun"
                        },
                        new Mission()
                        {
                            DisplayName = "Missile Boat Diplomacy"
                        },
                        new Mission()
                        {
                            DisplayName = "Rebel Assault"
                        },
                        new Mission()
                        {
                            DisplayName = "Eliminate TIE Defender Factory"
                        },
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty XI: Hunt for Zaarin",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Intercept Convoy"
                        },
                        new Mission()
                        {
                            DisplayName = "Preemptive Strike"
                        },
                        new Mission()
                        {
                            DisplayName = "Bait and Switch"
                        },
                        new Mission()
                        {
                            DisplayName = "An Unexpected Attack"
                        },
                        new Mission()
                        {
                            DisplayName = "The Real Thing"
                        },
                        new Mission()
                        {
                            DisplayName = "Protect Vorknkx Project"
                        },
                        new Mission()
                        {
                            DisplayName = "Evacuate"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty XII: Prelude to Endor",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Escort Prison Ship"
                        },
                        new Mission()
                        {
                            DisplayName = "Escort Prisoners"
                        },
                        new Mission()
                        {
                            DisplayName = "Attack on Bothuwui"
                        },
                        new Mission()
                        {
                            DisplayName = "Strike on Kothlis"
                        },
                        new Mission()
                        {
                            DisplayName = "Bothan Treachery"
                        },
                        new Mission()
                        {
                            DisplayName = "Recon Military Summit"
                        },
                        new Mission()
                        {
                            DisplayName = "Delay Strike Force"
                        }
                    }
                },
                new Tour()
                {
                    TourName = "Tour of Duty XIII: The Emperor's Will",
                    Missions = new List<Mission>()
                    {
                        new Mission()
                        {
                            DisplayName = "Surprise Attack"
                        },
                        new Mission()
                        {
                            DisplayName = "Capture the Turncoat"
                        },
                        new Mission()
                        {
                            DisplayName = "Track Down Rebels"
                        },
                        new Mission()
                        {
                            DisplayName = "Missile Boat Trouble"
                        },
                        new Mission()
                        {
                            DisplayName = "Return to Vorknkx"
                        },
                        new Mission()
                        {
                            DisplayName = "Corvette Attack"
                        },
                        new Mission()
                        {
                            DisplayName = "Zaarin Takes the Bait"
                        },
                        new Mission()
                        {
                            DisplayName = "The Trap is Sprung"
                        }
                    }
                }
            };

            for (var i = 0; i < tours.Count; i++)
            {
                var tour = tours[i];
                var tourId = tour.TourName.Replace(" ", "_");

                var rndDate = new DateTime(2017, new Random().Next(1, 13), new Random().Next(1,28), new Random().Next(1,12), new Random().Next(1,60), new Random().Next(1,60)).ToUniversalTime();
                for (var ii = 0; ii < tour.Missions.Count; ii++)
                {
                    var mission = tour.Missions[ii];
                    var mEntity = new Entity()
                    {
                        [nameof(Mission.DisplayName)] = new Value()
                        {
                            StringValue = mission.DisplayName
                        },
                        [nameof(Mission.PositionInTour)] = new Value()
                        {
                            IntegerValue = ii
                        },
                        [nameof(Mission.MissionBriefing)] = new Value()
                        {
                            StringValue = ""
                        },
                        [nameof(Mission.LastPlayedOn)] = rndDate,
                        [nameof(Mission.TourId)] = tourId
                    };
                    mEntity.Key = dbContext.MissionsKeyFactory.CreateKey($"{ii}-{mission.DisplayName.Replace(" ", "_")}");
                    missionEntities.Add(mEntity);
                }

                tourEntities.Add(new Entity()
                {
                    Key = tourKeyFactory.CreateKey(tourId),
                    [nameof(Tour.TourName)] = new Value()
                    {
                        StringValue = tour.TourName
                    },
                    [nameof(Tour.Position)] = i
                });
            }

            var tourResults = await dbContext.Db.UpsertAsync(tourEntities);
            var missionResults = await dbContext.Db.UpsertAsync(missionEntities);

            // Upsert users

            var userKeyFactory = dbContext.Db.CreateKeyFactory(userKindName);
            var userEntities = new List<Entity>();
            var users = new List<User>();

            var existingUsers = userManager.Users.ToList();
            foreach (var u in existingUsers)
            {
                var shipUnlocked = new List<string>();
                var tieFighter = dbContext.Db.RunQuery(new Query("Ship")
                {
                    Filter = Filter.Equal("DisplayName", new Value()
                    {
                        StringValue = "TIE-Fighter"
                    })
                });
                shipUnlocked.Add(tieFighter.Entities?[0]?["DisplayName"]?.StringValue);

                users.Add(new User()
                {
                    Id = u.Id,
                    MedalsWon = new List<Medal>(),
                    ShipsUnlocked = shipUnlocked
                });
            }

            foreach (var u in users)
            {
                var userMedalEntities = new Entity[0];
                var userShipEntities = new Entity[u.ShipsUnlocked.Count];
                for (var i = 0; i < u.ShipsUnlocked.Count; i++)
                {
                    userShipEntities[i] = new Entity()
                    {
                        ["ShipId"] = new Value()
                        {
                            StringValue = u.ShipsUnlocked[i]
                        }
                    };
                }

                userEntities.Add(new Entity()
                {
                    Key = userKeyFactory.CreateKey(u.Id),
                    ["MedalsWon"] = new Value()
                    {
                        ArrayValue = userMedalEntities,
                        //ExcludeFromIndexes = true
                    },
                    ["ShipsUnlocked"] = new Value()
                    {
                        ArrayValue = userShipEntities,
                        //ExcludeFromIndexes = true
                    }
                });
            }

            await dbContext.Db.UpsertAsync(userEntities);
        }

        #endregion

        #endregion
    }
}
