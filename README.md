# Audio Visualiser
 A game using Audio interactions and procedural generation to paint a picture

In this project I wished to paint a picture, an endless road listening to music. 
Originally I wanted to be miving past the generated mesh nad endlessly generate the mesh but unfortunatly when I started going down this route the project would lag to much and cause it to crash.

I procedurally generated the land and the y axis is random based on Noise. The white line is also procedurally generated.

I am most proud of the particle effects I have created that react to audio. The way it works is that there is an invisible object that a group of particles follow. It moves based on the audio. The light emission is also affected by audio levels that allow it to glow and dimer with the music. 

One big problem I faced was that towards the upper ends of frequences the music never reached that point so there was not much input in objects that react to these higher frequencys. I created 8 sub sections that emcompass the different levels. the higher the hertz the more that were grouped togther. This allowed for the higher levels of hertz to be grouped together and a reaction is bigger.

I used knowledge from class and from a few forums to create the generated mesh.

For the particles I related to a tutorial on youtube
[![YouTube](https://www.youtube.com/vi/uLoquo79GQI&t=103s/0.jpg)](https://www.youtube.com/watch?v=uLoquo79GQI&t=103s)

My Video
[![YouTube](https://www.youtube.com/vi/Ue7GECXJWes/0.jpg)](https://www.youtube.com/watch?v=Ue7GECXJWes)
