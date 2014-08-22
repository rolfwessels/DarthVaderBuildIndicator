using System.Collections.Generic;

namespace BuildIndicatron.Shared.Models.Composition
{
    public class Choreography
    {
	    public Choreography()
	    {
			Sequences= new List<Sequences>();
	    }

	    public List<Sequences> Sequences { get; set; }
    }
}