using Microsoft.AspNetCore.Mvc;

[Route("/ping")]
public class PingController : ControllerBase
{

    [HttpGet()]
    public ActionResult Ping()
    {
        return Ok();
    }
}