// MODAL
function showModal(modal) {
    modal.addClass('active');
    $('body').addClass('modal-active');
}
function hideModal(modal) {
    modal.removeClass('active');
    $('body').removeClass('modal-active');
}
$(document).on('click', '[data-modal]', function() {
    var modalId = $(this).attr('data-modal');
    showModal($('#' + modalId));
});
$(document).on('click', '.modal-close', function() {
    hideModal($(this).closest('.modal'));
});
// HIDE MODAL ON CLICK OUTSIDE
$(document).on('click', '.modal', function(e) {
    if (e.target !== this) {
        return;
    }
    hideModal($(this));
});
// SHARE BUTTON
// if (navigator.share) {
//     $(document).on('click', '.share-btn', function(e) {
//         e.preventDefault();
//         console.log('s')
//         var url = $(this).attr('data-url');
//         var text = $(this).attr('data-text');
//         // CHECK FOR NAVIGATOR SHARE
//         navigator.share({
//             title: text,
//             url: url
//         });
//     });
// } else {
//     // REMOVE SHARE BUTTON
//     $('.share-btn').remove();
// }