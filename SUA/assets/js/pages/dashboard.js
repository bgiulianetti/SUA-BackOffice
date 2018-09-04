FROM = moment().subtract(7, 'days').format("YYYY-MM-DD");
TO = moment().format("YYYY-MM-DD");

$(".fromDate").html(FROM)
$(".toDate").html(TO)

$.ajax({
  url: "/dashboard/getLogins/"+FROM+"/"+TO,
  type: 'get',
  success: function(response){
    $(".logins").html(response.value)
  },
  error: function (response) {
    console.log(response)
  }
});

$.ajax({
  url: "/dashboard/getSubscriptions/"+FROM+"/"+TO,
  type: 'get',
  success: function(response){
    $(".subscriptions").html(response.value)
  },
  error: function (response) {
    console.log(response)
  }
});

$.ajax({
  url: "/dashboard/getSignups/"+FROM+"/"+TO,
  type: 'get',
  success: function(response){
    $(".signups").html(response.value)
  },
  error: function (response) {
    console.log(response)
  }
});


getConversion = function(){
  s = parseInt($(".subscriptions").html())
  t = parseInt($(".signups").html())
  $(".conversion").html(((s*100)/t))
}


if( $('#headline-chart').length > 0 ) {
  $.ajax({
    url: "/dashboard/getGraph/"+FROM+"/"+TO,
    type: 'get',
    success: function(response){
      console.log(response)
      arrayDate = [
        {day: moment().subtract(7, 'days').format("DD") , month: moment().subtract(7, 'days').format("MM")},
        {day: moment().subtract(6, 'days').format("DD") , month: moment().subtract(6, 'days').format("MM")},
        {day: moment().subtract(5, 'days').format("DD") , month: moment().subtract(5, 'days').format("MM")},
        {day: moment().subtract(4, 'days').format("DD") , month: moment().subtract(4, 'days').format("MM")},
        {day: moment().subtract(3, 'days').format("DD") , month: moment().subtract(3, 'days').format("MM")},
        {day: moment().subtract(2, 'days').format("DD") , month: moment().subtract(2, 'days').format("MM")},
        {day: moment().subtract(1, 'days').format("DD") , month: moment().subtract(1, 'days').format("MM")},
        {day: moment().format("DD") , month: moment().format("MM")}
      ]

      for (var i = 0; i < arrayDate.length; i++) {
        for (var k = 0; k < response.logins.length; k++) {
          if(parseInt(response.logins[k].day) == parseInt(arrayDate[i].day)) {
            arrayDate[i].logins = response.logins[k].value
          }else if( arrayDate[i].logins == undefined ) arrayDate[i].logins = 0
        }
        for (var k = 0; k < response.signups.length; k++) {
          if(parseInt(response.signups[k].day) == parseInt(arrayDate[i].day)) {
            arrayDate[i].logins = response.signups[k].value
          }else if( arrayDate[i].logins == undefined ) arrayDate[i].logins = 0
        }
        for (var k = 0; k < response.subscriptions.length; k++) {
          if(parseInt(response.subscriptions[k].day) == parseInt(arrayDate[i].day)) {
            arrayDate[i].logins = response.subscriptions[k].value
          }else if( arrayDate[i].logins == undefined ) arrayDate[i].logins = 0
        }
      }
      returnDates = function(){
        dates = []
        for (var i = 0; i < arrayDate.length; i++) {
          dates[dates.length] = arrayDate[i].day+"/"+arrayDate[i].month
        }
        return dates
      }
      returnLogins = function(){
        values = []
        for (var i = 0; i < arrayDate.length; i++) {
          values[values.length] = arrayDate[i].logins
        }
        return values
      }
      returnSignups = function(){
        values = []
        for (var i = 0; i < arrayDate.length; i++) {
          values[values.length] = arrayDate[i].signups
        }
        return values
      }
      returnSubscriptions = function(){
        values = []
        for (var i = 0; i < arrayDate.length; i++) {
          values[values.length] = arrayDate[i].subscriptions
        }
        return values
      }

      var data = {
        labels: returnDates(),
        series: [
          returnLogins(),
          returnSignups(),
          returnSubscriptions()
        ]
      };

      var options = {
        height: 300,
        showArea: true,
        showLine: false,
        showPoint: false,
        fullWidth: true,
        axisX: {
          showGrid: false
        },
        lineSmooth: false,
      };

      new Chartist.Line('#headline-chart', data, options);

    },

    error: function (response) {
      console.log(response)
    }
  });


}
