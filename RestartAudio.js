function startAudioOnClick(){
 
    document.querySelector('#unity-container').addEventListener('click', function() {
        context.resume().then(() => {
          console.log('Playback resumed successfully');
        });
      });
}