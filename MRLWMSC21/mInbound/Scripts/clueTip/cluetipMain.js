   <script type="text/javascript">
    $(document).ready(function() {
   
        $('a.helpWTitle').cluetip({
            splitTitle: '|', // use the invoking element's title attribute to populate the clueTip...
            // ...and split the contents into separate divs where there is a "|"
            showTitle: true // show the clueTip's heading
        });

        $('a.help').cluetip({
            splitTitle: '|', // use the invoking element's title attribute to populate the clueTip...
            // ...and split the contents into separate divs where there is a "|"
            showTitle: false // hide the clueTip's heading
        });
    });
</script>
