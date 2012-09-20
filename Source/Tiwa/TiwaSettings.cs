
namespace Tiwa
{
    using Atom.Configuration;

    public sealed class TiwaSettings : Config
    {
        [ConfigProperty( DefaultValue = 50.0 )]
        public double Time
        {
            get;
            set;
        }

        public TiwaSettings()
            : base( new PlainTextConfigStore( "settings.txt" ) )
        {
        }
    }
}
