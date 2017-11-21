﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.Extensions.Configuration;
using Google.Apis.Services;

namespace TieFighter.Models
{
    public class TieFighterDatastoreContext
    {
        #region Constructors

        public TieFighterDatastoreContext(IConfiguration configuration)
        {
            string projectId = configuration["Authentication:Google:ProjectId"];
            db = DatastoreDb.Create(projectId);
            
            medals = new List<Medal>();
            ships = new List<Ship>();
            tours = new List<Tours>();
            users = new List<ApplicationUserDatastoreModel>();
        }

        #endregion

        #region Fields

        private readonly DatastoreDb db;
        private readonly IList<Medal> medals;
        private readonly IList<Ship> ships;
        private readonly IList<Tours> tours;
        private readonly IList<ApplicationUserDatastoreModel> users;

        private const string medalKindName = "Medal";
        private const string shipKindName = "Ship";
        private const string tourKindName = "Tour";
        private const string missionKindName = "Mission";
        private const string userKindName = "User";

        #endregion

        #region Properties

        #endregion

        #region Functions

        #region Seed db with data

        public static async Task InitializeDbAsync(IConfiguration configuration, TieFighterContext db)
        {
            var dbContext = new TieFighterDatastoreContext(configuration);
            var medalKeyFactory = dbContext.db.CreateKeyFactory(medalKindName);

            // Upsert medals
            var medalEntities = new List<Entity>();
            var medals = new List<Medal>()
            {
                new Medal()
                {
                    MedalName = "Double Kill",
                    Description = "Kill two enemies within two seconds of each other.",
                    PointsWorth = 2
                },
                new Medal()
                {
                    MedalName = "Triple Kill",
                    Description = "Kill three enemies within two seconds of each other.",
                    PointsWorth = 2
                },
                new Medal()
                {
                    MedalName = "Ace",
                    Description = "Get five confirmed kills in one life.",
                    PointsWorth = 5
                },
                new Medal()
                {
                    MedalName = "Double Ace",
                    Description = "Get ten confirmed kills in one life.",
                    PointsWorth = 10
                }
            };

            foreach (var medal in medals)
            {
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
                    }
                });
            }

            await dbContext.db.UpsertAsync(medalEntities);

            // Upsert Ships
            var shipKeyFactory = dbContext.db.CreateKeyFactory(shipKindName);

            var shipEntities = new List<Entity>();
            var ships = new List<Ship>()
            {
                new Ship()
                {
                    FileLocation = "TieFighter",
                    DisplayName = "TIE-Fighter"
                },
                new Ship()
                {
                    FileLocation = "TieBomber",
                    DisplayName = "TIE-Bomber"
                },
                new Ship()
                {
                    FileLocation = "TieIntercepter",
                    DisplayName = "TIE-Intercepter"
                },
                new Ship()
                {
                    FileLocation = "StarDestroyerMark1",
                    DisplayName = "Imperial I - Star Destroyer"
                },
                new Ship()
                {
                    FileLocation = "XWing",
                    DisplayName = "X-Wing"
                },
                new Ship()
                {
                    FileLocation = "YWing",
                    DisplayName = "Y-Wing"
                },
                new Ship()
                {
                    FileLocation = "AWing",
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

            await dbContext.db.UpsertAsync(shipEntities);

            // Upsert tours/missions
            var tourKeyFactory = dbContext.db.CreateKeyFactory(tourKindName);
            var tourEntities = new List<Entity>();
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

                var missions = new List<Entity>();
                for (var ii = 0; ii < tour.Missions.Count; ii++)
                {
                    var mission = tour.Missions[ii];
                    missions.Add(new Entity()
                    {
                        ["DisplayName"] = new Value()
                        {
                            StringValue = mission.DisplayName
                        },
                        ["PositionInTour"] = new Value()
                        {
                            IntegerValue = ii
                        },
                        ["MissionBriefing"] = new Value()
                        {
                            StringValue = ""
                        }
                    });
                }

                tourEntities.Add(new Entity()
                {
                    Key = tourKeyFactory.CreateKey(tour.TourName.Replace(" ", "_")),
                    ["DisplayName"] = new Value()
                    {
                        StringValue = tour.TourName
                    },
                    ["Missions"] = new Value()
                    {
                        ArrayValue = missions.ToArray()
                    }
                });
            }

            await dbContext.db.UpsertAsync(tourEntities);

            // Upsert users

            var userKeyFactory = dbContext.db.CreateKeyFactory(userKindName);
            var userEntities = new List<Entity>();
            var users = new List<ApplicationUserDatastoreModel>();

            var existingUsers = db.Users.ToList();
            foreach (var u in existingUsers)
            {
                var shipUnlocked = new List<int>();
                var tieFighter = dbContext.db.RunQuery(new Query("Ship")
                {
                    Filter = Filter.Equal("DisplayName", new Value()
                    {
                        StringValue = "TIE-Fighter"
                    })
                });
                shipUnlocked.Add(int.Parse(tieFighter.Entities?[0]["Id"].StringValue));

                users.Add(new ApplicationUserDatastoreModel()
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
                            IntegerValue = u.ShipsUnlocked[i]
                        }
                    };
                }

                userEntities.Add(new Entity()
                {
                    Key = userKeyFactory.CreateKey(u.Id),
                    ["MedalsWon"] = new Value()
                    {
                        ArrayValue = userMedalEntities,
                        ExcludeFromIndexes = true
                    },
                    ["ShipsUnlocked"] = new Value()
                    {
                        ArrayValue = userShipEntities,
                        ExcludeFromIndexes = true
                    }
                });
            }

            await dbContext.db.UpsertAsync(userEntities);
        }

        #endregion

        #endregion
    }
}