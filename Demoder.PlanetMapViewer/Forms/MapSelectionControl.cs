using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class MapSelectionControl : UserControl
    {
        public MapSelectionControl()
        {
            InitializeComponent();
            API.UiElements.MapList = this.MapComboBox;
        }

        private void MapSelectionControl_Load(object sender, EventArgs e)
        {

        }

        private void SelectMap()
        {
            if (API.MapManager == null) { return; }

            if (this.RadioAuto.Checked == true)
            {
                return;
            }

            API.MapManager.FindAvailableMaps(API.State.MapType);
            API.MapManager.SelectMap(API.State.MapType);
            API.UiElements.TileDisplay.Focus();
        }

        #region Event handling
        
        private void RadioMapTypeCheckedChanged(object sender, EventArgs e)
        {
            // User clicking on a RadioButton will send two statechange messages.
            // One for the unchecked and one for the checked RadioButton.
            // We only need to process one of the events.
            if (!((RadioButton)sender).Checked)
            {
                return;
            }

            // Automatic selection
            if (this.RadioAuto.Checked)
            {
                API.State.MapTypeAutoSwitching = true;
                return;
            }

            // RK selected
            if (this.RadioRK.Checked)
            {
                API.State.MapType = MapType.Rubika;
            }
            // SL selected
            else if (this.RadioSL.Checked)
            {
                API.State.MapType = MapType.Shadowlands;
            }

            this.SelectMap();
            API.State.MapTypeAutoSwitching = false;
            return;
        }

        private void MapComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (API.MapManager == null) { return; }
            API.UiElements.VScrollBar.Value = 0;
            API.UiElements.HScrollBar.Value = 0;

            var mapInfo = this.MapComboBox.SelectedItem as MapSelectionItem;
            if (mapInfo == null) { return; }
            if (mapInfo.Type == MapType.Rubika)
            {
                Properties.MapSettings.Default.SelectedRubikaMap = mapInfo.MapPath;
            }
            else
            {
                Properties.MapSettings.Default.SelectedShadowlandsMap = mapInfo.MapPath;
            }

            API.MapManager.SelectMap(mapInfo.MapPath);
            //API.Camera.AdjustScrollbarsToLayer();
            API.UiElements.TileDisplay.Focus();
        }
        #endregion
    }
}
