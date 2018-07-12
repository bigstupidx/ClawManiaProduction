<?php
$attachment_location = "ClawMania.unity3d";
 header($_SERVER["SERVER_PROTOCOL"] . " 200 OK");
 header("Cache-Control: public");
 header("Content-Type: application/zip");
 header("Content-Transfer-Encoding: Binary");
 header("Content-Length:".filesize($attachment_location));
 header("Content-Disposition: attachment; filename=ClawMania.unity3d");
 readfile($attachment_location);
 Die();
?>