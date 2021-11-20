$(document).ready(function() {
	
  	$('#copyLink').click(function(){
			$('#copiedIndicator').show();	
			$('#copiedIndicator').addClass('animated bounceOut');	
			setTimeout(function() {
        $('#copiedIndicator').hide();
				$('#copiedIndicator').removeClass('animated bounceOut');
    	}, 1000);
		});

		$('#dataTable').DataTable({

			"fnDrawCallback": function (oSettings) {
				$('[data-toggle="tooltip"]').tooltip({
						container: 'body'
				});
			}

		});
	
		
		$("#open01").click(function(){
			$("#emailBody01").slideToggle(300);
			$(this).toggleClass('active');
		});
		
		$("#closePopup").click(function(){
			$(".chatPopup").slideToggle(300);
		});
		
		$('#chatTextarea').keypress( function(){ 
			$('.sendBtn').addClass('active');	
		});
		
		$('#endChat').click( function(){ 
			$('.endChatWrapper').show();	
		});
		
		$('#cancelChat').click( function(){ 
			$('.endChatWrapper').hide();	
		});
		
		$('#rejectBtn').click( function(){ 
			$('#removedRow').animate({
				height:0,
				opacity:0,
				});	
		});
		
		$("#chatScroll").nanoScroller({ scroll: 'bottom' });
		$("#sideScroll").nanoScroller();
		$(".screenListWrapper").nanoScroller();
	
    $(function() {
      $( ".datepicker" ).datepicker({
        numberOfMonths: 2,
        showButtonPanel: true
      });
    });  

     $(function () {
       $('[data-toggle="tooltip"]').tooltip({
				 container : 'body'
			 });
			 $('[rel="tooltip"]').tooltip({
				 container : 'body'
			 });
     });
	
		//$('.fg-button.ui-button.ui-state-default').click(function() {
//			$('[data-toggle="tooltip"]').tooltip({
//				 container : 'body'
//			 });
//		});

    $(function(){
      var height = $(document).height();
      $(".loginWrapper, .spinnerWrapper").height(height);
    }); 
    
		$(function(){
      var height = $(document).height();
      $(".messageContainer").height(height);
    }); 
    

    $('input[type=radio]').iCheck({
      checkboxClass: 'iradio_square-green',
      radioClass: 'iradio_square-green',
      increaseArea: '20%' // optional
    });
    $('input[type=checkbox]').iCheck({
      checkboxClass: 'icheckbox_square-green',
      radioClass: 'icheckbox_square-green',
      increaseArea: '20%' // optional
    });


    //Page Attachment
    $('#attchmentButton').click(function() {
      $(this).hide(); 
      $('#newAttachment').show('slow', function() {
      });
    });

    $('#closeAttachment').click(function() {
      $('#newAttachment').hide('slow');
      $('#attchmentButton').show('fast', function() {
      });
    });   
		 
		 
		$('.dd').nestable({ /* config options */ });
		
		$('.editor').click(function() {
			$(this).addClass('fullscreen');
			$('.minimizeBtn').show();
			window.scrollTo(0, 0);
			var height = $(window).height() - 140;
    	$(this).height(height);
		});
		$('.minimizeBtn').click(function() {
			$('.editor').removeClass('fullscreen');
			$('.editor').height('200');
			$('.minimizeBtn').hide();
			$("#s4-ribbonrow").hide();
		});
	
		$(".ms-rteTable-CollapsibleTable tr:nth-child(odd)").click(function() {
    	$(this).next().slideToggle(300);
			$(this).toggleClass('collapsed');
		});
});