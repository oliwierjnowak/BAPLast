using Microsoft.AspNetCore.Mvc;

namespace BAPLast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetController : ControllerBase
    {
        public static List<Tweet> tweets = new List<Tweet>()
        {
            new Tweet
            {
                creator = "oli123",
                content = "helloworld"
            },
            new Tweet
            {
                creator = "oli123",
                content = "i dont want bap"
            },
            new Tweet
            {
                creator = "oli123",
                content = "matura plsss"
            },
            new Tweet
            {
                creator = "drago",
                content = "cs cs cs"
            },
            new Tweet
            {
                creator = "drago",
                content = "drago bin ich"
            },
            new Tweet
            {
                creator = "drago",
                content = "matura plsss"
            }
        };

        [HttpGet]
        public IEnumerable<Tweet> Get()
        {
            return tweets;
        }
        [HttpGet("{creator}")]
        public ActionResult<IEnumerable<Tweet>> GetByCreator(string creator)
        {
            return Ok(tweets.Where(c => c.creator.Equals(creator)));
        }

        [HttpPost]
        public ActionResult<Tweet> Post([FromBody]Tweet tweet)
        {
            if (String.IsNullOrEmpty(tweet.content) || String.IsNullOrEmpty(tweet.creator))
            {
                return BadRequest();
            }
            return tweet;
        }

    }
}
