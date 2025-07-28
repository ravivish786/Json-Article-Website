var linkCache = [];
var hoverTimers = {};
var currentCard = null;

// Mouseenter on article link
$(document).on('mouseenter', '.article-details a', function () {
    let me = $(this);
    let url = me.attr('href');

    // Start 500ms hover timer
    hoverTimers[url] = setTimeout(function () {
        // Remove any existing preview
        $('.article-preview-card').remove();

        let offset = me.offset();

        // Show loader card immediately
        let loaderCard = $(`
            <div class="card article-preview-card">
                <div class="card-body">
                        <div class="d-flex justify-content-center">
                          <span class="fw-bold me-3"> Loading...</span>
                          <div class="spinner-border" role="status">
                            <span class="visually-hidden">Loading...</span>
                          </div>
                        </div>
                </div>
            </div>
        `);
        $('body').append(loaderCard);
        loaderCard.css({
            top: offset.top + me.outerHeight() + 10,
            left: offset.left
        });
        currentCard = loaderCard;

        // Check cache
        let cached = linkCache.find(item => item.url === url);
        if (cached) {
            showArticleData(cached.data, me);
        } else {
            $.ajax({
                url: '/article/scrape',
                type: 'GET',
                data: { url: encodeURIComponent(url) },
                success: function (data) {
                    console.log('Fetched data:', data);
                    linkCache.push({ url, data });
                    showArticleData(data, me);
                },
                error: function () {
                    showArticleData({ title: 'Error', description: 'Could not load article.' }, me);
                }
            });
        }
    }, 500);
});

// Cancel fetch on mouse leave before 500ms
$(document).on('mouseleave', '.article-details a', function () {
    let url = $(this).attr('href');
    if (hoverTimers[url]) {
        clearTimeout(hoverTimers[url]);
        delete hoverTimers[url];
    }
});

// Click outside to close
$(document).on('click', function (e) {
    if (!$(e.target).closest('.article-preview-card, .article-details a').length) {
        $('.article-preview-card').remove();
        currentCard = null;
    }
});

// Optional: mouseleave from card also removes it
$(document).on('mouseleave', '.article-preview-card', function () {
    $(this).remove();
    currentCard = null;
});

// Show full data
function showArticleData(data, targetElement) {
    if (!currentCard) return;

    // If no useful data, remove the card
    if (!data.title && !data.description && !data.image) {
        if (currentCard) {
            currentCard.remove(); // Remove from DOM
            currentCard = null;   // Clear reference
        }
        $('.article-preview-card').remove(); // Redundant but safe
        return;
    }


    let newCard = $(`
        <div class="card article-preview-card">
            ${data.image ? `<img src="${data.image}" onerror='this.remove()' class="card-img-top" alt="...">` : ''}
            <div class="card-body">
                <h6 class="card-title">${data.title}</h6>
                <p class="card-text mb-0 lh-sm">
                    <small>${data.description}</span>
                </p>
            </div>
        </div>
    `);

    let offset = $(targetElement).offset();
    newCard.css({
        top: offset.top + $(targetElement).outerHeight() + 10,
        left: offset.left,
        position: 'absolute',
        zIndex: 9999
    });

    $('.article-preview-card').remove();
    $('body').append(newCard);
    currentCard = newCard;
}