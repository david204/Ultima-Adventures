using System;
using System.Collections;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server.Gumps
{
    public class NameChangeGump : Gump
    {
        const string DEFAULT_NAME = "Escribe aquí...";
        public NameChangeGump(Mobile from) : base(100, 100)
        {
            // Immobilize the player
            from.CantWalk = true;

            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(137, 119, 334, 195, 9250);
            AddBackground(221, 264, 171, 29, 3000);
            AddHtml(153, 135, 304, 113, @"El nombre que has elegido ya está en uso y no se encuentra disponible. Debes elegir otro nombre para continuar. Borra el texto de abajo e introduce un nuevo nombre de fantasía.", (bool)true, (bool)false);

            AddLabel(153, 270, 0, @"Nuevo nombre:");
            AddTextEntry(224, 268, 163, 21, 0, 1, DEFAULT_NAME, 16); // 16 Character Limit
            AddButton(395, 267, 4023, 4024, 1, GumpButtonType.Reply, 0); // Okay
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (from == null)
            {
                return;
            }

            string name = GetString(info, 1);
            name = name != null ? name.Trim() : null;
            if (!string.IsNullOrWhiteSpace(name) && name != DEFAULT_NAME)
            {
                if (!NameVerification.Validate(name, 2, 16, true, false, true, 1, NameVerification.SpaceOnly))
                {
                    from.SendMessage(0X22, "Ese nombre no es aceptable o ya está en uso.");
                    from.SendGump(new NameChangeGump(from));
                    return;
                }
                if (CharacterCreation.CheckDupe(from, name))
                {
                    from.SendMessage(0X22, "Tu nombre ahora es {0}.", name);
                    from.Name = name;
                    from.CantWalk = false;
                    return;
                }
            }
            else
            {
                from.SendMessage(0X22, "Debes introducir un nombre.");
            }

            from.SendGump(new NameChangeGump(from));
        }
    }
}