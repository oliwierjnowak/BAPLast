package computerdatabase;

import io.gatling.javaapi.core.*;
import io.gatling.javaapi.http.*;

import static io.gatling.javaapi.core.CoreDsl.*;
import static io.gatling.javaapi.http.HttpDsl.http;
public class TweetStressTest extends Simulation {
    private HttpProtocolBuilder httpTweet = http.baseUrl("http://localhost:8080")
            .acceptHeader("application/json")
            .contentTypeHeader("application/json");

    private static ChainBuilder createTweet =
            exec(http("create some tweet")
                    .post("http://localhost:8080/tweet")
                    .body(ElFileBody("bodies/tweet.json")).asJson());

    private static ChainBuilder getAllTweets =
            exec(http("getAllTweets")
                    .get("http://localhost:8080/tweet"));
    private static ChainBuilder getSingleTweet =
            exec(http("create many")
                    .get("http://localhost:8080/tweet/cernil"));

    private ScenarioBuilder setupTweets =scenario("setup Tweet").exec(createTweet);



    private ScenarioBuilder tweetsMany = scenario("get list ")
            .exec(getAllTweets)
            .pause(2)
            .exec(getSingleTweet);

    {
        setUp(


                                setupTweets.injectOpen(atOnceUsers(1))
                                        .protocols(httpTweet).andThen(tweetsMany.injectOpen(atOnceUsers(10)))



        );
    }
}
