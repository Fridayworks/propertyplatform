const COMPARE_KEY = 'property_compare_list';
const MAX_COMPARE = 4;

const ComparisonTool = {
    getIds() {
        const data = localStorage.getItem(COMPARE_KEY);
        return data ? JSON.parse(data) : [];
    },

    toggle(id, title, imageUrl) {
        let ids = this.getIds();
        const index = ids.findIndex(item => item.id === id);

        if (index > -1) {
            ids.splice(index, 1);
        } else {
            if (ids.length >= MAX_COMPARE) {
                alert(`You can only compare up to ${MAX_COMPARE} properties.`);
                return false;
            }
            ids.push({ id, title, imageUrl });
        }

        localStorage.setItem(COMPARE_KEY, JSON.stringify(ids));
        this.updateUI();
        return true;
    },

    remove(id) {
        let ids = this.getIds();
        ids = ids.filter(item => item.id !== id);
        localStorage.setItem(COMPARE_KEY, JSON.stringify(ids));
        this.updateUI();
    },

    clear() {
        localStorage.removeItem(COMPARE_KEY);
        this.updateUI();
    },

    updateUI() {
        const ids = this.getIds();
        const bar = document.getElementById('comparison-bar');
        const countEl = document.getElementById('compare-count');
        const listEl = document.getElementById('compare-thumbnails');

        if (!bar) return;

        if (ids.length > 0) {
            bar.classList.remove('translate-y-full');
            countEl.innerText = ids.length;
            
            // Update Compare Now button URL
            const compareBtn = bar.querySelector('a[href="/Compare"]');
            if (compareBtn) {
                const idString = ids.map(i => i.id).join(',');
                compareBtn.href = `/Compare?ids=${idString}`;
            }

            listEl.innerHTML = ids.map(item => `
                <div class="relative group h-12 w-12 rounded-lg border border-gray-200 overflow-hidden bg-white shadow-sm">
                    <img src="${item.imageUrl || '/img/placeholder.jpg'}" class="h-full w-full object-cover" />
                    <button onclick="ComparisonTool.remove('${item.id}')" class="absolute -top-1 -right-1 bg-red-500 text-white rounded-full h-4 w-4 flex items-center justify-center text-[10px] opacity-0 group-hover:opacity-100 transition-opacity">×</button>
                </div>
            `).join('');

            // Update checkboxes on page
            document.querySelectorAll('.compare-toggle').forEach(el => {
                const id = el.getAttribute('data-id');
                el.checked = ids.some(item => item.id === id);
            });
        } else {
            bar.classList.add('translate-y-full');
        }
    }
};

// Initial update
document.addEventListener('DOMContentLoaded', () => ComparisonTool.updateUI());
