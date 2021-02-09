using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgApp.Shared.Types.Enums
{
    public class ModalSyles
    {
        private const string ModalFramed = "rpgui-content rpgui-modal framed";
        private const string ModalGolden = "rpgui-content rpgui-modal framedgolden";
        private const string ModalGolden2 = "rpgui-content rpgui-modal framedgolden2";
        private const string ModelGrey = "rpgui-content rpgui-modal framedgrey";

        /// <summary>
        /// Framed style modal (grey background, gold frame)
        /// </summary>
        /// <param name="size">set size of modal using string const options (ex: Framed(ModalSize.Large))
        /// or strings directly (ex: Framed("large"))</param>
        /// <returns>css class string for modal display</returns>
        public static string Framed(string size)
        {
            var modalsize = $"modal-{size}";
            return $"{ModalFramed} {modalsize}";
        }
        /// <summary>
        /// GoldenFramed style modal (brown background, gold frame)
        /// </summary>
        /// <param name="size">set size of modal using string const options (ex: GoldenFramed(ModalSize.Large))
        /// or strings directly (ex: GoldenFramed("large"))</param>
        /// <returns>css class string for modal display</returns>
        public static string GoldenFramed(string size)
        {
            var modalsize = $"modal-{size}";
            return $"{ModalGolden} {modalsize}";
        }
        /// <summary>
        /// GoldenFramed2 style modal (gold background, rough gold frame)
        /// </summary>
        /// <param name="size">set size of modal using string const options (ex: GoldenFramed2(ModalSize.Large))
        /// or strings directly (ex: GoldenFramed2("large"))</param>
        /// <returns>css class string for modal display</returns>
        public static string GoldenFramed2(string size)
        {
            var modalsize = $"modal-{size}";
            return $"{ModalGolden2} {modalsize}";
        }
        /// <summary>
        /// FramedGrey style modal (grey background, rough grey frame)
        /// </summary>
        /// <param name="size">set size of modal using string const options (ex: FramedGrey(ModalSize.Large))
        /// or strings directly (ex: FramedGrey("large"))</param>
        /// <returns>css class string for modal display</returns>
        public static string FramedGrey(string size)
        {
            var modalsize = $"modal-{size}";
            return $"{ModelGrey} {modalsize}";
        }
    }

    public class ModalSize
    {
        public const string ExtraLarge = "xlarge";
        public const string Large = "large";
        public const string Medium = "medium";
        public const string Small = "small";
        public const string ExtraSmall = "xsmall";

    }
}
