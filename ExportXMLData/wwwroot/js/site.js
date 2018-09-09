
jQuery(document).ready(function () {

    $("#GetXML").click(function () {
        searchRunning = true;
        var totaltext = $('#totalExpense').val();
        getTotalCost(totaltext);
    });
});
searchRunning = false;

//Ajax call to retrieve data from REST service
function getTotalCost(alltext) {
    $("#messageInfo").html("Loading Weather from API...");
    $("#messageInfo").css("visibility", "");
    var data = JSON.stringify({ alltext: alltext });
    $.ajax({
        url: '/api/GetXMLData',
        type: 'POST',
        data: data,
        contentType: 'application/json',
        error: function (errorThrown) {
            $("#messageInfo").html("Api is not available at the moment");
            searchRunning = false;
        },
        success: function (result) {
            $("#messageInfo").css("visibility", "hidden");
            searchRunning = false;
            if (result.xmlDataPair !=="") {              
                obj = JSON.parse(result.xmlDataPair);
                var jsonObject = JSON.stringify(obj.Result, undefined, 2);
                $("#returnData").html(jsonObject);  
                $('#total').html(result.totalExcludingGST);
                $('#gst').html(result.gst);
                $('#costCentre').html(result.costCentre);
            }
            else {
                $("#returnData").html(result.error); 
                $("#expenseInfo").html("");
            }
        }
    }).done(function () {       
        searchRunning = false;
    });
}