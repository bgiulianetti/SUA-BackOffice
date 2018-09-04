


function getYoutubeIdAndShowImage(){
  var url = $("#videoUrl").val();
  if(url != ""  && url.indexOf("v=") > -1) {
    var video_id = $("#videoUrl").val().split("v=")[1].substring(0, 11);
    $("#videoThumbnail").attr("src","http://img.youtube.com/vi/" + video_id + "/1.jpg");
    $("#videoThumbnail").show();
  }
  else {
    $("#videoThumbnail").hide();
    $("#videoThumbnail").attr("src","");
  }
}