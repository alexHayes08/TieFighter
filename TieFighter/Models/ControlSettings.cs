using System;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;
using TieFighter.Models.Settings;

namespace TieFighter.Models
{
    public class ControlSettings : IDatastoreEntityAndJsonBinding
    {
        private string UserId { get; set; }
        public Input[] Forward { get; set; }
        public Input[] Left { get; set; }
        public Input[] Reverse { get; set; }
        public Input[] Right { get; set; }
        public Input[] Pitch { get; set; }
        public Input[] Yaw { get; set; }
        public Input[] Roll { get; set; }
        public Input[] PrimaryFire { get; set; }
        public Input[] SecondaryFire { get; set; }
        public Input[] SwitchWeapons { get; set; }

        private Input[] ParseInputEntity (Entity entity, string propertyName)
        {
            var inputs = new List<Input>();

            foreach (var forward in entity.Properties[propertyName].ArrayValue.Values)
            {
                if (!string.IsNullOrEmpty(forward.StringValue))
                {
                    var mouseBttn = (MouseButtons)Enum.Parse(typeof(MouseButtons), forward.StringValue);
                    inputs.Add(new Input()
                    {
                        MouseButton = mouseBttn
                    });
                }
                else if (forward.DoubleValue > 0)
                {
                    inputs.Add(new Input()
                    {
                        KeyCode = Convert.ToInt32(forward.IntegerValue)
                    });
                }
            }
            return inputs.ToArray();
        }

        private ArrayValue ConvertInputToEntity(Input[] inputs)
        {
            var arrVal = new List<Entity>();
            foreach (var input in inputs)
            {
                var stringVal = Enum.GetName(typeof(MouseButtons), input.MouseButton);
                if (!string.IsNullOrEmpty(stringVal))
                {
                    arrVal.Add(new Entity()
                    {
                        [nameof(Input.MouseButton)] = stringVal
                    });
                }
                else
                {
                    arrVal.Add(new Entity()
                    {
                        [nameof(Input.KeyCode)] = input.KeyCode
                    });
                }
            }

            return arrVal.ToArray();
        }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var controlSetting = new ControlSettings
            {
                UserId = entity.Key.Path[0].Name,
                Forward = ParseInputEntity(entity, nameof(Forward)),
                Left = ParseInputEntity(entity, nameof(Left)),
                Reverse = ParseInputEntity(entity, nameof(Reverse)),
                Right = ParseInputEntity(entity, nameof(Right)),
                Pitch = ParseInputEntity(entity, nameof(Pitch)),
                Yaw = ParseInputEntity(entity, nameof(Yaw)),
                Roll = ParseInputEntity(entity, nameof(Roll)),
                PrimaryFire = ParseInputEntity(entity, nameof(PrimaryFire)),
                SecondaryFire = ParseInputEntity(entity, nameof(SecondaryFire)),
                SwitchWeapons = ParseInputEntity(entity, nameof(SwitchWeapons))
            };

            return controlSetting;
        }

        public override Entity ToEntity()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                throw new Exception("Cannot convert this to entity " +
                    "without first setting the UserId property.");
            }

            var key = DatastoreDbReference.ControlSettingsKeyFactory.CreateKey(UserId);
            var entity = new Entity()
            {
                Key = key,
                [nameof(Forward)] = ConvertInputToEntity(Forward),
                [nameof(Left)] = ConvertInputToEntity(Left),
                [nameof(Reverse)] = ConvertInputToEntity(Reverse),
                [nameof(Right)] = ConvertInputToEntity(Right),
                [nameof(Pitch)] = ConvertInputToEntity(Pitch),
                [nameof(Yaw)] = ConvertInputToEntity(Yaw),
                [nameof(Roll)] = ConvertInputToEntity(Roll),
                [nameof(PrimaryFire)] = ConvertInputToEntity(PrimaryFire),
                [nameof(SecondaryFire)] = ConvertInputToEntity(SecondaryFire),
                [nameof(SwitchWeapons)] = ConvertInputToEntity(SwitchWeapons)
            };

            return entity;
        }
    }
}
